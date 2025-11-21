using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.WinForm
{
    public partial class DeleteProfile : Form
    {
        //IProfileDaoAsync _profileApiClient = new ProfileApiClient("https://LocalHost:7204");
        IProfileDaoAsync _profileApiClient = new InMemoryProfileDaoStub();
        public DeleteProfile() => InitializeComponent();

        private async void DeleteProfile_Load(object sender, EventArgs e) => LoadProfiles();
        private void lstProfiles_SelectedIndexChanged(object sender, EventArgs e) => UpdateUi();
        private void btnDelete_Click(object sender, EventArgs e) => DeleteSelectedProfile();


        private void LoadProfiles()
        {
            lstProfiles.Items.Clear();
            var profiles = _profileApiClient.GetAllAsync().Result;
            foreach (var profile in profiles)
            {
                lstProfiles.Items.Add(profile);
            }
            UpdateUi();
        }

        private void UpdateUi()
        {
            btnDelete.Enabled = lstProfiles.SelectedItem != null;
        }
        private void DeleteSelectedProfile()
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
            _profileApiClient.DeleteAsync(selectedProfile.ProfileId);

            lstProfiles.Items.Remove(selectedProfile);

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
