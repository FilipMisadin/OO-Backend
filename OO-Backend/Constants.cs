using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OO_Backend
{
    public class Constants
    {
        public static string JwtKey = "1Nm9rMLkG5NCM5UqhfvZu4xfWKfceIU1NTUelY964Qu1bp3oR3qlqhJXJZ1QwCJrEcSHmX";

        public static string DogOwnerDoesntExistError = "Owner doesn't exist.";
        public static string NotificationDoesntExistError = "Notification doesn't exist.";
        public static string DogDoesntExistError = "Dog doesn't exist.";
        public static string NameIsRequiredError = "Name is required.";
        public static string ReceiveUserIsNotValidError = "Receive User is not valid.";
        public static string UserCantNotifyHimselfError = "User can't notify himself.";
        public static string UserCantRateHimselfError = "User can't rate himself.";
        public static string UsernameAlreadyExistError = "Username already exists.";
        public static string WrongUserIdError = "Wrong user id.";
        public static string WrongDogOwnerError = "Dog doesn't belong to user.";

        public static string DefaultImageUrl = "https://i.imgur.com/JGu6U1X.jpg";
    }
}
