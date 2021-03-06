﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OO_Backend.Responses;
using OO_Backend.Enums;
using System.Security.Claims;

namespace OO_Backend.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<OfferAd> OfferServicesAds { get; set; }
        public DbSet<RequestAd> RequestServicesAds { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<OfferNotification> OfferNotifications { get; set; }
        public DbSet<RequestNotification> RequestNotifications { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<OfferToNeighborhood> OfferToNeighborhood { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OfferNotification>()
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (NotificationStatus)Enum.Parse(typeof(NotificationStatus), v));
            modelBuilder
                .Entity<RequestNotification>()
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (NotificationStatus)Enum.Parse(typeof(NotificationStatus), v));
            modelBuilder
                .Entity<OfferAd>()
                .Property(p => p.DayAvailableFrom)
                .HasConversion(
                    v => v.ToString(),
                    v => (WeekDay)Enum.Parse(typeof(WeekDay), v));
            modelBuilder
                .Entity<OfferAd>()
                .Property(p => p.DayAvailableTo)
                .HasConversion(
                    v => v.ToString(),
                    v => (WeekDay)Enum.Parse(typeof(WeekDay), v));
            modelBuilder.Entity<OfferToNeighborhood>()
                .HasKey(c => new { c.NeighborhoodId, c.OfferId });

            modelBuilder.Entity<User>()
                .HasMany(c => c.Dogs)
                .WithOne(e => e.Owner)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public List<User> GetUsers() => Users.ToList();
        public List<Dog> GetDogs() => Dogs.ToList();
        public List<OfferAd> GetOfferServicesAds() => OfferServicesAds.ToList();
        public List<RequestAd> GetRequestServicesAds() => RequestServicesAds.ToList();
        public List<Review> GetReviews() => Reviews.ToList();
        public List<OfferNotification> GetOfferNotifications() => OfferNotifications.ToList();
        public List<Neighborhood> GetNeighborhoods() => Neighborhoods.ToList();
        public List<OfferToNeighborhood> GetOfferToNeighborhoods() => OfferToNeighborhood.ToList();
        public List<RequestNotification> GetRequestNotifications() => RequestNotifications.ToList();


        public void UpdateUser(User user)
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
        }

        public void UpdateDog(Dog dog)
        {
            Entry(GetDogs().Find(o => o.Id == dog.Id)).State = EntityState.Detached;
            Dogs.Update(dog);
            this.SaveChanges();
        }

        public OfferAd UpdateOfferServicesAd(OfferAdBody ad)
        {
            Entry(GetOfferServicesAds().Find(o => o.Id == ad.Id)).State = EntityState.Detached;
            var offer = OfferServicesAds.Update(ad.ToModel()).Entity;
            this.SaveChanges();
            ad.Neighborhoods.ForEach(neighborhood =>
            {
                int neighborhoodId;
                if (!NeighborhoodExists(neighborhood))
                {
                    var neighborhoodModel = new Neighborhood { Id = 0, Name = neighborhood };
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

        public void UpdateRequestServicesAd(RequestAd ad)
        {
            Entry(GetRequestServicesAds().Find(o => o.Id == ad.Id)).State = EntityState.Detached;
            RequestServicesAds.Update(ad);
            this.SaveChanges();
        }

        public void UpdateReview(Review review)
        {
            Entry(GetReviews().Find(o => o.Id == review.Id)).State = EntityState.Detached;
            Reviews.Update(review);
            this.SaveChanges();
        }

        public void UpdateOfferNotification(OfferNotification notification)
        {
            Entry(GetOfferNotifications().Find(o => o.Id == notification.Id)).State = EntityState.Detached;
            OfferNotifications.Update(notification);
            this.SaveChanges();
        }

        public void UpdateRequestNotification(RequestNotification requestNotification)
        {
            Entry(GetRequestNotifications().Find(o => o.Id == requestNotification.Id)).State = EntityState.Detached;
            RequestNotifications.Update(requestNotification);
            this.SaveChanges();
        }

        public int UpdateNeighborhood(Neighborhood neighborhood)
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
        }

        public void AddUser(User user)
        {
            Users.Add(user);
            this.SaveChanges();
        }

        public void AddDog(Dog dog)
        {
            Dogs.Add(dog);
            this.SaveChanges();
        }

        public OfferAd AddOfferServicesAd(OfferAdBody ad)
        {
            ad.PostDate = DateTime.Now;
            var offer = OfferServicesAds.Add(ad.ToModel()).Entity;
            this.SaveChanges();
            ad.Neighborhoods.ForEach(neighborhood =>
            {
                int neighborhoodId;
                if (!NeighborhoodExists(neighborhood))
                {
                    var neighborhoodModel = new Neighborhood { Id = 0, Name = neighborhood };
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

        public void AddRequestServicesAd(RequestAd ad)
        {
            ad.PostDate = DateTime.Now;
            RequestServicesAds.Add(ad);
            this.SaveChanges();
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
            this.SaveChanges();
        }

        public int AddOfferNotification(OfferNotification notification)
        {
            notification.PostDate = DateTime.Now;
            var entity = OfferNotifications.Add(notification).Entity;
            this.SaveChanges();
            return entity.Id;
        }

        public int AddRequestNotification(RequestNotification notification)
        {
            notification.PostDate = DateTime.Now;
            var entity = RequestNotifications.Add(notification).Entity;
            this.SaveChanges();
            return entity.Id;
        }

        private int AddNeighborhood(Neighborhood neighborhood)
        {
            var hood = Neighborhoods.Add(neighborhood);
            this.SaveChanges();
            return hood.Entity.Id;
        }

        private void AddOfferToNeighborhood(OfferToNeighborhood offerToNeighborhood)
        {
            OfferToNeighborhood.Add(offerToNeighborhood);
            this.SaveChanges();
        }

        public void RemoveUser(User user)
        {
            var dogs = GetUserDogs(user.Id);
            dogs.ForEach(RemoveDog);

            var offerNotifications = GetUserRelatedOfferNotifications(user.Id);
            offerNotifications.ForEach(RemoveOfferNotification);

            var requestNotifications = GetUserRelatedRequestNotifications(user.Id);
            requestNotifications.ForEach(RemoveRequestNotification);

            var offerAds = GetUserOfferServicesAds(user.Id);
            offerAds.ForEach(RemoveOfferServicesAd);

            var requestAds = GetUserRequestServicesAds(user.Id);
            requestAds.ForEach(RemoveRequestServicesAd);

            var reviews = GetUserRelatedReviews(user.Id);
            reviews.ForEach(RemoveReview);

            Users.Remove(user);
            this.SaveChanges();
        }

        public void RemoveDog(Dog dog)
        {
            var requestAds = GetDogRequestServicesAds(dog.Id);
            requestAds.ForEach(RemoveRequestServicesAd);

            var requestNotifications = GetDogRequestNotifications(dog.Id);
            requestNotifications.ForEach(RemoveRequestNotification);

            Dogs.Remove(dog);
            this.SaveChanges();
        }

        public void RemoveOfferServicesAd(OfferAd ad)
        {
            var offerToNeighborhoods = GetOfferToNeighborhoodFromOffer(ad.Id);
            offerToNeighborhoods.ForEach(RemoveOfferToNeighborhood);

            var requestNotifications = GetRequestNotificationsFromOffer(ad.Id);
            requestNotifications.ForEach(RemoveRequestNotification);

            OfferServicesAds.Remove(ad);
            this.SaveChanges();
        }

        public void RemoveRequestServicesAd(RequestAd ad)
        {
            var offerNotifications = GetOfferNotificationsFromOffer(ad.Id);
            offerNotifications.ForEach(RemoveOfferNotification);

            RequestServicesAds.Remove(ad);
            this.SaveChanges();
        }

        public void RemoveReview(Review review)
        {
            Reviews.Remove(review);
            this.SaveChanges();
        }

        public void RemoveOfferNotification(OfferNotification notification)
        {
            GetOfferToNeighborhoodFromOffer(notification.Id).ForEach(offerToNeighborhood =>
            {
                GetOfferToNeighborhoods().Remove(offerToNeighborhood);
            });

            OfferNotifications.Remove(notification);
            this.SaveChanges();
        }

        public void RemoveRequestNotification(RequestNotification requestNotification)
        {
            RequestNotifications.Remove(requestNotification);
            this.SaveChanges();
        }

        public void RemoveNeighborhood(Neighborhood neighborhood)
        {
            var offerToNeighborhoods = GetOfferToNeighborhoodFromNeighborhood(neighborhood.Id);
            offerToNeighborhoods.ForEach(RemoveOfferToNeighborhood);

            Neighborhoods.Remove(neighborhood);
            this.SaveChanges();
        }

        private void RemoveOfferToNeighborhood(OfferToNeighborhood offerToNeighborhood)
        {
            OfferToNeighborhood.Remove(offerToNeighborhood);
            this.SaveChanges();
        }

        public void AcceptOfferNotification(long notificationId)
        {
            var notification = GetOfferNotification(notificationId);
            notification.Status = NotificationStatus.Accepted;
            UpdateOfferNotification(notification);
        }

        public void AcceptRequestNotification(long notificationId)
        {
            var notification = GetRequestNotification(notificationId);
            notification.Status = NotificationStatus.Accepted;
            UpdateRequestNotification(notification);
        }

        public void DeclineOfferNotification(long notificationId)
        {
            var notification = GetOfferNotification(notificationId);
            notification.Status = NotificationStatus.Declined;
            UpdateOfferNotification(notification);
        }

        public void DeclineRequestNotification(long notificationId)
        {
            var notification = GetRequestNotification(notificationId);
            notification.Status = NotificationStatus.Declined;
            UpdateRequestNotification(notification);
        }

        public User GetUser(long id)
        {
            return GetUsers().Find(user => user.Id == id);
        }

        public User GetUser(string username)
        {
            return GetUsers().Find(user => user.Username == username);
        }

        public List<Neighborhood> GetOfferNeighborhoods(long offerId)
        {
            var offerToNeighborhood = GetOfferToNeighborhoods().FindAll(n => n.OfferId == offerId);

            var responseList = new List<Neighborhood>();
            offerToNeighborhood.ForEach(o =>
            {
                responseList.Add(GetNeighborhoods().Find(n => n.Id == o.NeighborhoodId));
            });

            return responseList;
        }

        private List<OfferToNeighborhood> GetOfferToNeighborhoodFromOffer(long offerId)
        {
            return GetOfferToNeighborhoods().FindAll(o => o.OfferId == offerId);
        }

        private List<RequestNotification> GetRequestNotificationsFromOffer(long offerId)
        {
            return GetRequestNotifications().FindAll(o => o.OfferAdId == offerId);
        }

        private List<OfferNotification> GetOfferNotificationsFromOffer(long offerId)
        {
            return GetOfferNotifications().FindAll(o => o.RequestAdId == offerId);
        }

        private List<OfferToNeighborhood> GetOfferToNeighborhoodFromNeighborhood(long neighborhoodId)
        {
            return GetOfferToNeighborhoods().FindAll(o => o.NeighborhoodId == neighborhoodId);
        }

        public OfferAd GetOfferServicesAd(long id)
        {
            return GetOfferServicesAds().Find(o => o.Id == id);
        }

        public Dog GetDog(long id)
        {
            return GetDogs().Find(dog => dog.Id == id);
        }

        public List<Dog> GetUserDogs(long userId)
        {
            return GetDogs().FindAll(dog => dog.OwnerId == userId);
        }

        public OfferNotification GetOfferNotification(long id)
        {
            return GetOfferNotifications().Find(notification => notification.Id == id);
        }

        public RequestNotification GetRequestNotification(long id)
        {
            return GetRequestNotifications().Find(notification => notification.Id == id);
        }

        public List<RequestNotificationResponse> GetUserRequestNotifications(long userId)
        {
            var responds = GetRequestNotifications().FindAll(notifications => notifications.ReceivedUserId == userId);
            var respondResponses = new List<RequestNotificationResponse>();

            responds.ForEach(respond =>
            {
                respondResponses.Add(respond.ToResponse(this));
            });
            return respondResponses;
        }

        public List<OfferNotificationResponse> GetUserOfferNotifications(long userId)
        {
            var responds = GetOfferNotifications().FindAll(notifications => notifications.ReceivedUserId == userId);
            var respondResponses = new List<OfferNotificationResponse>();

            responds.ForEach(respond =>
            {
                respondResponses.Add(respond.ToResponse(this));
            });
            return respondResponses;
        }

        private List<RequestNotification> GetDogRequestNotifications(long dogId)
        {
            return GetRequestNotifications().FindAll(o => o.DogId == dogId);
        }

        private List<OfferNotification> GetUserRelatedOfferNotifications(long userId)
        {
            return GetOfferNotifications().FindAll(notifications => notifications.ReceivedUserId == userId || notifications.SendUserId == userId);
        }

        private List<RequestNotification> GetUserRelatedRequestNotifications(long userId)
        {
            return GetRequestNotifications().FindAll(notifications => notifications.ReceivedUserId == userId || notifications.SendUserId == userId);
        }

        public List<OfferNotificationResponse> GetUserOfferResponds(long userId)
        {
            var responds = GetOfferNotifications().FindAll(notifications => notifications.SendUserId == userId);
            var respondResponses = new List<OfferNotificationResponse>();

            responds.ForEach(respond =>
            {
                respondResponses.Add(respond.ToResponse(this));
            });
            return respondResponses;
        }

        public List<RequestNotificationResponse> GetUserRequestResponds(long userId)
        {
            var responds = GetRequestNotifications().FindAll(notifications => notifications.SendUserId == userId);
            var respondResponses = new List<RequestNotificationResponse>();

            responds.ForEach(respond =>
            {
                respondResponses.Add(respond.ToResponse(this));
            });
            return respondResponses;
        }

        public List<ReviewResponse> GetUserReviews(long userId)
        {
            var reviews = GetReviews().FindAll(review => review.ReceiveUserId == userId);

            var response = new List<ReviewResponse>();
            reviews.ForEach(review =>
            {
                response.Add(review.ToResponse(this));
            });

            return response;
        }

        private List<Review> GetUserRelatedReviews(long userId)
        {
            return GetReviews().FindAll(review => review.ReceiveUserId == userId || review.SendUserId == userId);
        }

        public Review GetReview(long id)
        {
            return GetReviews().Find(review => review.Id == id);
        }

        public RequestAd GetRequestServicesAd(long id)
        {
            return GetRequestServicesAds().Find(o => o.Id == id);
        }

        public List<RequestAd> GetUserRequestServicesAds(long userId)
        {
            return GetRequestServicesAds().FindAll(o => o.UserId == userId);
        }

        private List<RequestAd> GetDogRequestServicesAds(long dogId)
        {
            return GetRequestServicesAds().FindAll(o => o.DogId == dogId);
        }

        public List<OfferAd> GetUserOfferServicesAds(long userId)
        {
            return GetOfferServicesAds().FindAll(o => o.UserId == userId);
        }

        public bool OfferNotificationExists(long id)
        {
            return GetOfferNotifications().Any(e => e.Id == id);
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

        private bool NeighborhoodExists(string name)
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
            this.SaveChanges();
        }
    }
}