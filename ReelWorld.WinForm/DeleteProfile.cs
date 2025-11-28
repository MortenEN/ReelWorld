using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.WinForm
{
    public partial class DeleteProfile : Form
    {
        private System.Windows.Forms.Timer _refreshTimer;

        IProfileDaoAsync _profileApiClient = new ProfileApiClient("https://LocalHost:7204");
        //IProfileDaoAsync _profileApiClient = new InMemoryProfileDaoStub();
        public DeleteProfile()
        {
            InitializeComponent();
            SetupRefreshTimer();
        }

        private async void DeleteProfile_Load(object sender, EventArgs e) => await LoadProfiles();
        private void lstProfiles_SelectedIndexChanged(object sender, EventArgs e) => UpdateUi();
        private void btnDelete_Click(object sender, EventArgs e) => DeleteSelectedProfile();

        private void SetupRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 3000; // 3 seconds
            _refreshTimer.Tick += async (s, e) => await LoadProfiles();
            _refreshTimer.Start();
        }

        private async Task LoadProfiles()
        {
            var profiles = await _profileApiClient.GetAllAsync();

            // Hvis antal og alle profiler matcher => ingen opdatering
            if (lstProfiles.Items.Count == profiles.Count() &&
                lstProfiles.Items.Cast<Profile>().Select(p => p.ProfileId)
                    .SequenceEqual(profiles.Select(p => p.ProfileId)))
                return;

            // Gem selection
            var selectedProfile = lstProfiles.SelectedItem as Profile;

            // Opdater liste
            lstProfiles.Items.Clear();
            foreach (var profile in profiles)
                lstProfiles.Items.Add(profile);

            // Genskab selection
            if (selectedProfile != null)
            {
                var itemToSelect = lstProfiles.Items.Cast<Profile>()
                                    .FirstOrDefault(p => p.ProfileId == selectedProfile.ProfileId);
                if (itemToSelect != null)
                    lstProfiles.SelectedItem = itemToSelect;
            }

            UpdateUi();
        }


        private void ClearUi()
        {
            lblName.Text = "";
            lblEmail.Text = "";
            lblPhone.Text = "";
            lblCity.Text = "";
            txtBoxDescription.Text = "";
        }

        private void UpdateUi()
        {
            btnDelete.Enabled = lstProfiles.SelectedItem != null;
            if (lstProfiles.SelectedItem is Profile profile)
            {
                lblName.Text = profile.Name;
                lblEmail.Text = profile.Email;
                lblPhone.Text = profile.PhoneNo;
                lblCity.Text = profile.City;
                txtBoxDescription.Text = profile.Description;
            }
        }
        private async void DeleteSelectedProfile()
        {
            if (lstProfiles.SelectedIndex == -1)
            {
                return;
            }
            if (MessageBox.Show("Do you want to delete ?", "Delete?", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            var selectedIndex = lstProfiles.SelectedIndex;
            var selectedProfile = (Profile)lstProfiles.SelectedItem;
            await _profileApiClient.DeleteAsync(selectedProfile.ProfileId);

            lstProfiles.Items.Remove(selectedProfile);

            if (lstProfiles.Items.Count == 0)
            {
                ClearUi();
                return;
            }

            //in case the deleted item was last in the list
            //move selected index up
            int maxIndex = lstProfiles.Items.Count - 1;
            if (selectedIndex > maxIndex)
            {
                selectedIndex = maxIndex;
            }
            lstProfiles.SelectedIndex = selectedIndex;
        }

    }
}
