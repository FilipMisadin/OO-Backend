using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend
{
    public static class Constants
    {
        public const string DogOwnerDoesntExistError = "Owner doesn't exist.";
        public const string NotificationDoesntExistError = "Notification doesn't exist.";
        public const string DogDoesntExistError = "Dog doesn't exist.";
        public const string NameIsRequiredError = "Name is required.";
        public const string ReceiveUserIsNotValidError = "Receive User is not valid.";
        public const string UserCantNotifyHimselfError = "User can't notify himself.";
        public const string UserCantRateHimselfError = "User can't rate himself.";
        public const string UsernameAlreadyExistError = "Username already exists.";
        public const string WrongUserIdError = "Wrong user id.";
        public const string WrongDogOwnerError = "Dog doesn't belong to user.";
        public const string PasswordRequiredError = "Password is required.";

        public const string DefaultImageUrl = "https://i.imgur.com/JGu6U1X.jpg";
    }
}
