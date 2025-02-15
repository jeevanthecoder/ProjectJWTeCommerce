namespace ProjectJWTeCommerce.Models.DTOs
{
    public class UpdateUserDTO
    {
        public int userId {  get; set; }
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
