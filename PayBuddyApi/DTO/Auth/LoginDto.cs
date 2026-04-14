namespace PayBuddyApi.DTO.Auth
{

    public class LoginForSaveDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginForUpdateDTO : LoginForSaveDTO
    {

    }

    public class LoginDto : LoginForUpdateDTO
    {

    }

    public class LoginDTOMinusRelations : LoginForUpdateDTO
    {

    }

}
