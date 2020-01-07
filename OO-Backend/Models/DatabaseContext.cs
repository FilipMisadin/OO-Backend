using System;
using System.Collections.Generic;
using System.Linq;
using OO_Backend.Models;
using Microsoft.EntityFrameworkCore;
using OO_Backend.Responses;
using OO_Backend.Enums;
using System.Security.Claims;

namespace OO_Backend.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<DogModel> Dogs { get; set; }
        public DbSet<OfferServicesAdModel> OfferServicesAds { get; set; }
        public DbSet<RequestServicesAdModel> RequestServicesAds { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }
        public DbSet<NeighborhoodModel> Neighborhoods { get; set; }
        public DbSet<OfferToNeighborhood> OfferToNeighborhood { get; set; }
        public DbSet<RequestNotificationModel> RequestNotifications { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<NotificationModel>()
                .Property(p => p.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (NotificationType)Enum.Parse(typeof(NotificationType), v));
            modelBuilder
                .Entity<NotificationModel>()
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (NotificationStatus)Enum.Parse(typeof(NotificationStatus), v));
            modelBuilder
                .Entity<OfferServicesAdModel>()
                .Property(p => p.DayAvailableFrom)
                .HasConversion(
                    v => v.ToString(),
                    v => (WeekDay)Enum.Parse(typeof(WeekDay), v));
            modelBuilder
                .Entity<OfferServicesAdModel>()
                .Property(p => p.DayAvailableTo)
                .HasConversion(
                    v => v.ToString(),
                    v => (WeekDay)Enum.Parse(typeof(WeekDay), v));
            modelBuilder.Entity<OfferToNeighborhood>()
                .HasKey(c => new { c.NeighborhoodId, c.OfferId });
        }

        public List<UserModel> GetUsers() => Users.ToList();
        public List<DogModel> GetDogs() => Dogs.ToList();
        public List<OfferServicesAdModel> GetOfferServicesAds() => OfferServicesAds.ToList();
        public List<RequestServicesAdModel> GetRequestServicesAds() => RequestServicesAds.ToList();
        public List<ReviewModel> GetReviews() => Reviews.ToList();
        public List<NotificationModel> GetNotifications() => Notifications.ToList();
        public List<NeighborhoodModel> GetNeighborhoods() => Neighborhoods.ToList();
        public List<OfferToNeighborhood> GetOfferToNeighborhoods() => OfferToNeighborhood.ToList();
        public List<RequestNotificationModel> GetRequestNotifications() => RequestNotifications.ToList();


        public void UpdateUser(UserModel user)
        {
            var currentUser = GetUsers().Find(o => o.Id == user.Id);
            Entry(currentUser).State = EntityState.Detached;
            user.Rating = currentUser.Rating;

            if(user.Password == "")
            {
                user.Password = currentUser.Password;
            }

            Users.Update(user);
            this.SaveChanges();
            return;
        }

        public void UpdateDog(DogModel dog)
        {
            Entry(GetDogs().Find(o => o.Id == dog.Id)).State = EntityState.Detached;
            Dogs.Update(dog);
            this.SaveChanges();
            return;
        }

        public OfferServicesAdModel UpdateOfferServicesAd(OfferAdBodyModel ad)
        {
            Entry(GetOfferServicesAds().Find(o => o.Id == ad.Id)).State = EntityState.Detached;
            var offer = OfferServicesAds.Update(Converters.OfferAdBodyToOfferAdModel(ad)).Entity;
            this.SaveChanges();
            ad.Neighborhoods.ForEach(neighborhood =>
            {
                var neighborhoodId = 0;
                if (!NeighborhoodExists(neighborhood))
                {
                    var neighborhoodModel = new NeighborhoodModel { Id = 0, Name = neighborhood };
                    neighborhoodId = AddNeighborhood(neighborhoodModel);
                }
                else
                {
                    neighborhoodId = GetNeighborhoods().Find(o => o.Name == neighborhood).Id;
                }
                var offerToNeighborhood = new OfferToNeighborhood
                {
                    NeighborhoodId = neighborhoodId,
                    OfferId = offer.Id
                };
                if (!OfferToNeighborhoodExists(offerToNeighborhood))
                {
                    AddOfferToNeighborhood(offerToNeighborhood);
                }
            });
            this.SaveChanges();
            return offer;
        }

        public void UpdateRequestServicesAd(RequestServicesAdModel ad)
        {
            Entry(GetRequestServicesAds().Find(o => o.Id == ad.Id)).State = EntityState.Detached;
            RequestServicesAds.Update(ad);
            this.SaveChanges();
            return;
        }

        public void UpdateReview(ReviewModel review)
        {
            Entry(GetReviews().Find(o => o.Id == review.Id)).State = EntityState.Detached;
            Reviews.Update(review);
            this.SaveChanges();
            return;
        }

        public void UpdateNotification(NotificationModel notification)
        {
            Entry(GetNotifications().Find(o => o.Id == notification.Id)).State = EntityState.Detached;
            Notifications.Update(notification);
            this.SaveChanges();
            return;
        }

        public void AcceptNotification(long notificationId)
        {
            var notification = GetNotification(notificationId);
            notification.Status = NotificationStatus.Accepted;
            UpdateNotification(notification);
            return;
        }

        public void DeclineNotification(long notificationId)
        {
            var notification = GetNotification(notificationId);
            notification.Status = NotificationStatus.Declined;
            UpdateNotification(notification);
            return;
        }

        public void UpdateRequestNotification(RequestNotificationModel requestNotification)
        {
            Entry(GetRequestNotifications().Find(o => o.Id == requestNotification.Id)).State = EntityState.Detached;
            RequestNotifications.Update(requestNotification);
            this.SaveChanges();
            return;
        }

        public int UpdateNeighborhood(NeighborhoodModel neighborhood)
        {
            Entry(GetNeighborhoods().Find(o => o.Id == neighborhood.Id)).State = EntityState.Detached;
            var hood = Neighborhoods.Update(neighborhood);
            this.SaveChanges();
            return hood.Entity.Id;
        }

        public void UpdateOfferToNeighborhood(OfferToNeighborhood offerToNeighborhood)
        {
            Entry(GetOfferToNeighborhoods().Find(o => o.NeighborhoodId == offerToNeighborhood.NeighborhoodId && o.OfferId == offerToNeighborhood.OfferId)).State = EntityState.Detached;
            OfferToNeighborhood.Update(offerToNeighborhood);
            this.SaveChanges();
            return;
        }

        public void AddUser(UserModel user)
        {
            Users.Add(user);
            this.SaveChanges();
            return;
        }

        public void AddDog(DogModel dog)
        {
            Dogs.Add(dog);
            this.SaveChanges();
            return;
        }

        public OfferServicesAdModel AddOfferServicesAd(OfferAdBodyModel ad)
        {
            var offer = OfferServicesAds.Add(Converters.OfferAdBodyToOfferAdModel(ad)).Entity;
            this.SaveChanges();
            ad.Neighborhoods.ForEach(neighborhood =>
            {
                var neighborhoodId = 0;
                if (!NeighborhoodExists(neighborhood))
                {
                    var neighborhoodModel = new NeighborhoodModel { Id = 0, Name = neighborhood };
                    neighborhoodId = AddNeighborhood(neighborhoodModel);
                }
                else
                {
                    neighborhoodId = GetNeighborhoods().Find(o => o.Name == neighborhood).Id;
                }
                AddOfferToNeighborhood(new OfferToNeighborhood
                {
                    NeighborhoodId = neighborhoodId,
                    OfferId = offer.Id
                });
            });
            this.SaveChanges();
            return offer;
        }

        public void AddRequestServicesAd(RequestServicesAdModel ad)
        {
            RequestServicesAds.Add(ad);
            /*if (!NeighborhoodExists(ad.Neighborhood))
            {
                var neighborhoodModel = new NeighborhoodModel { Id = 0, Name = ad.Neighborhood };
                AddNeighborhood(neighborhoodModel);
            }*/
            this.SaveChanges();
            return;
        }

        public void AddReview(ReviewModel review)
        {
            Reviews.Add(review);
            this.SaveChanges();
            return;
        }

        public int AddNotification(NotificationModel notification)
        {
            var entity = Notifications.Add(notification).Entity;
            this.SaveChanges();
            return entity.Id;
        }

        public void AddRequestNotification(RequestNotificationModel requestNotification, int notificationId)
        {
            requestNotification.NotificationId = notificationId;
            RequestNotifications.Add(requestNotification);
            this.SaveChanges();
            return;
        }

        public int AddNeighborhood(NeighborhoodModel neighborhood)
        {
            var hood = Neighborhoods.Add(neighborhood);
            this.SaveChanges();
            return hood.Entity.Id;
        }

        public void AddOfferToNeighborhood(OfferToNeighborhood offerToNeighborhood)
        {
            OfferToNeighborhood.Add(offerToNeighborhood);
            this.SaveChanges();
            return;
        }

        public void RemoveUser(UserModel user)
        {
            var dogs = GetUserDogs(user.Id);
            dogs.ForEach(dog => 
            {
                RemoveDog(dog);
            });

            var notifications = GetUserRelatedNotifications(user.Id);
            notifications.ForEach(notification =>
            {
                RemoveNotification(notification);
            });

            var offerAds = GetUserOfferServicesAds(user.Id);
            offerAds.ForEach(offerAd =>
            {
                RemoveOfferServicesAd(offerAd);
            });

            var requestAds = GetUserRequestServicesAds(user.Id);
            requestAds.ForEach(requestAd =>
            {
                RemoveRequestServicesAd(requestAd);
            });

            var reviews = GetUserRelatedReviews(user.Id);
            reviews.ForEach(review =>
            {
                RemoveReview(review);
            });

            Users.Remove(user);
            this.SaveChanges();
            return;
        }

        public void RemoveDog(DogModel dog)
        {
            var requestAds = GetDogRequestServicesAds(dog.Id);
            requestAds.ForEach(requestAd =>
            {
                RemoveRequestServicesAd(requestAd);
            });

            var requestNotifications = GetDogRequestNotifications(dog.Id);
            requestNotifications.ForEach(requestNotification =>
            {
                RemoveRequestNotification(requestNotification);
            });

            Dogs.Remove(dog);
            this.SaveChanges();
            return;
        }

        public void RemoveOfferServicesAd(OfferServicesAdModel ad)
        {
            var offerToNeighborhoods = GetOfferToNeighborhoodFromOffer(ad.Id);
            offerToNeighborhoods.ForEach(offerToNeighborhood =>
            {
                 RemoveOfferToNeighborhood(offerToNeighborhood);
            });

            OfferServicesAds.Remove(ad);
            this.SaveChanges();
            return;
        }

        public void RemoveRequestServicesAd(RequestServicesAdModel ad)
        {
            RequestServicesAds.Remove(ad);
            this.SaveChanges();
            return;
        }

        public void RemoveReview(ReviewModel review)
        {
            Reviews.Remove(review);
            this.SaveChanges();
            return;
        }

        public void RemoveNotification(NotificationModel notification)
        {
            var requestNotifications = GetRequestNotificationsFromNotification(notification.Id);
            requestNotifications.ForEach(requestNotification =>
            {
                RemoveRequestNotification(requestNotification);
            });

            Notifications.Remove(notification);
            this.SaveChanges();
            return;
        }

        public void RemoveRequestNotification(RequestNotificationModel requestNotification)
        {
            RequestNotifications.Remove(requestNotification);
            this.SaveChanges();
            return;
        }

        public void RemoveNeighborhood(NeighborhoodModel neighborhood)
        {
            var offerToNeighborhoods = GetOfferToNeighborhoodFromNeighborhood(neighborhood.Id);
            offerToNeighborhoods.ForEach(offerToNeighborhood =>
            {
                RemoveOfferToNeighborhood(offerToNeighborhood);
            });

            Neighborhoods.Remove(neighborhood);
            this.SaveChanges();
            return;
        }

        public void RemoveOfferToNeighborhood(OfferToNeighborhood offerToNeighborhood)
        {
            OfferToNeighborhood.Remove(offerToNeighborhood);
            this.SaveChanges();
            return;
        }

        public UserModel GetUser(long id)
        {
            return GetUsers().Find(user => user.Id == id);
        }

        public UserModel GetUser(string username)
        {
            return GetUsers().Find(user => user.Username == username);
        }

        public List<NeighborhoodModel> GetOfferNeighborhoods(long offerId)
        {
            var offerToNeighborhood = GetOfferToNeighborhoods().FindAll(n => n.OfferId == offerId);

            var responseList = new List<NeighborhoodModel>();
            offerToNeighborhood.ForEach(o =>
            {
                responseList.Add(GetNeighborhoods().Find(n => n.Id == o.NeighborhoodId));
            });

            return responseList;
        }

        public List<OfferToNeighborhood> GetOfferToNeighborhoodFromOffer(long offerId)
        {
            return GetOfferToNeighborhoods().FindAll(o => o.OfferId == offerId);
        }

        public List<OfferToNeighborhood> GetOfferToNeighborhoodFromNeighborhood(long neighborhoodId)
        {
            return GetOfferToNeighborhoods().FindAll(o => o.NeighborhoodId == neighborhoodId);
        }

        public OfferServicesAdModel GetOfferServicesAd(long id)
        {
            return GetOfferServicesAds().Find(o => o.Id == id);
        }

        public DogModel GetDog(long id)
        {
            return GetDogs().Find(dog => dog.Id == id);
        }

        public List<DogModel> GetUserDogs(long userId)
        {
            return GetDogs().FindAll(dog => dog.OwnerId == userId);
        }

        public NotificationModel GetNotification(long id)
        {
            return GetNotifications().Find(notification => notification.Id == id);
        }

        public RequestNotificationModel GetRequestNotification(long id)
        {
            return GetRequestNotifications().Find(notification => notification.Id == id);
        }

        public RequestNotificationModel GetNotificationRequestData(long notificationId)
        {
            return GetRequestNotifications().Find(notification => notification.NotificationId == notificationId);
        }

        public List<NotificationModel> GetUserNotifications(long userId)
        {
            return GetNotifications().FindAll(notifications => notifications.ReceivedUserId == userId);
        }

        public List<RequestNotificationModel> GetRequestNotificationsFromNotification(long notificationId)
        {
            return GetRequestNotifications().FindAll(o => o.NotificationId == notificationId);
        }

        public List<RequestNotificationModel> GetDogRequestNotifications(long dogId)
        {
            return GetRequestNotifications().FindAll(o => o.DogId == dogId);
        }

        public List<NotificationModel> GetUserRelatedNotifications(long userId)
        {
            return GetNotifications().FindAll(notifications => notifications.ReceivedUserId == userId || notifications.SendUserId == userId);
        }

        public List<NotificationModel> GetUserResponds(long userId)
        {
            return GetNotifications().FindAll(notifications => notifications.SendUserId == userId);
        }

        public List<ReviewModel> GetUserReviews(long userId)
        {
            return GetReviews().FindAll(review => review.ReceiveUserId == userId);
        }

        public List<ReviewModel> GetUserRelatedReviews(long userId)
        {
            return GetReviews().FindAll(review => review.ReceiveUserId == userId || review.SendUserId == userId);
        }

        public ReviewModel GetReview(long id)
        {
            return GetReviews().Find(review => review.Id == id);
        }

        public RequestServicesAdModel GetRequestServicesAd(long id)
        {
            return GetRequestServicesAds().Find(o => o.Id == id);
        }

        public List<RequestServicesAdModel> GetUserRequestServicesAds(long userId)
        {
            return GetRequestServicesAds().FindAll(o => o.UserId == userId);
        }
        
        public List<RequestServicesAdModel> GetDogRequestServicesAds(long dogId)
        {
            return GetRequestServicesAds().FindAll(o => o.DogId == dogId);
        }

        public List<OfferServicesAdModel> GetUserOfferServicesAds(long userId)
        {
            return GetOfferServicesAds().FindAll(o => o.UserId == userId);
        }

        public bool NotificationExists(long id)
        {
            return GetNotifications().Any(e => e.Id == id);
        }

        public bool RequestNotificationExists(long id)
        {
            return GetRequestNotifications().Any(e => e.Id == id);
        }

        public bool UserExists(long id)
        {
            return GetUsers().Any(e => e.Id == id);
        }

        public bool DogExists(long id)
        {
            return GetDogs().Any(e => e.Id == id);
        }

        public bool NeighborhoodExists(string name)
        {
            return GetNeighborhoods().Any(e => e.Name == name);
        }

        public bool NeighborhoodExists(long id)
        {
            return GetNeighborhoods().Any(e => e.Id == id);
        }

        public bool UsernameExists(string username)
        {
            return GetUsers().Any(o => o.Username == username);
        }

        public bool OfferServicesAdExists(long id)
        {
            return GetOfferServicesAds().Any(e => e.Id == id);
        }

        public bool ReviewExists(long id)
        {
            return GetReviews().Any(e => e.Id == id);
        }

        public bool RequestServicesAdExists(long id)
        {
            return GetRequestServicesAds().Any(e => e.Id == id);
        }

        public bool OfferToNeighborhoodExists(OfferToNeighborhood offerToNeighborhood)
        {
            return GetOfferToNeighborhoods().Any(e => e.NeighborhoodId == offerToNeighborhood.NeighborhoodId && e.OfferId == offerToNeighborhood.OfferId);
        }

        public bool IsOwner(long id, ClaimsPrincipal claims)
        {
            return claims.Identity.Name == GetUser(id).Username;
        }

        public void UpdateUserRating(long userId, float rating)
        {
            GetUser(userId).Rating = rating;
        }
    }
}