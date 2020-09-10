namespace EmployeeManagement.Application.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public string ExistingPhotoPath { get; set; }
        public int Id { get; set; }
    }
}