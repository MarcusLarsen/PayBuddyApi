namespace PayBuddyApi.DTO.User
{
    public class UserForSaveDTO
    {

    }

    public class UserForUpdateDTO : UserForSaveDTO
    {
        public required string Id { get; set; }
    }

    public class UserDto : UserForUpdateDTO
    {
        public required string UserName { get; set; }
    }

    public class UserDTOMinusRelations : UserForUpdateDTO
    {
        public required string UserName { get; set; }
    }
}
