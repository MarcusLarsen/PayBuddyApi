namespace PayBuddyApi.DTO.Auth
{
    public class RegisterForSaveDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterForUpdateDTO : RegisterForSaveDTO
    {

    }

    public class RegisterDto : RegisterForUpdateDTO
    {

    }

    public class RegisterDTOMinusRelations : RegisterForUpdateDTO
    {

    }
}
