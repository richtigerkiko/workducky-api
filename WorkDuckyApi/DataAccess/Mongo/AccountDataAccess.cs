using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkduckyLib.DataObjects.dbo.Mongo;
using WorkduckyLib.DataObjects.DummyData;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.Enums;

namespace WorkDuckyAPI.DataAccess.Mongo
{
    public class AccountDataAccess : MongoDBConn
    {

        private IMongoCollection<UserDocument> userCollection = null;

        public AccountDataAccess(IConfiguration configuration, ILogger logger) : base(configuration, logger)
        {
            userCollection = db.GetCollection<UserDocument>("users");
        }

        // Returns true if IUser exists in Database
        public bool DoesUserExist(string email)
        {
            var count = userCollection.CountDocuments(x => x.AuthenticationDocuments.Where(d => d.Username == email).Any());
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DoesUserExistAsync(string email)
        {
            var count = await userCollection.CountDocumentsAsync(x => x.AuthenticationDocuments.Where(d => d.Username == email).Any());
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task WriteEmailPasswordUserToDatabaseAsync(UserRegistrationEmailPasswordRequest user)
        {
            if (DoesUserExist(user.Email))
            {
                throw new Exception("User already exists");
            }
            var userDocument = new UserDocument()
            {
                AuthenticationDocuments = new List<AuthenticationDocument>()
                {
                    new AuthenticationDocument(){
                        AuthType = AuthenticationTypes.UsernamePassword,
                        Username = user.Email,
                        Password = user.Password,
                        isBlocked = false
                    }
                },
                UserSettings = GenerateDummySettings.DummySettings()
            };
            await userCollection.InsertOneAsync(userDocument);
        }

        public async Task<UserDocument> GetDBUserByEmail(string Email)
        {
            var userDocument = await userCollection.FindAsync(x => x.AuthenticationDocuments.Where(d => d.Username == Email).Any());
            var result = userDocument.FirstOrDefault();
            if (result == null)
            {
                throw new Exception("User doesn't exist");
            }
            else
            {
                return result;
            }
        }

        public async Task SetUserActive(string email)
        {
            var userDocument = userCollection.Find(x => x.AuthenticationDocuments.Where(d => d.AuthType == AuthenticationTypes.UsernamePassword && d.Username == email).Any()).FirstOrDefault();
            var authDocument = userDocument.AuthenticationDocuments.Where(d => d.AuthType == AuthenticationTypes.UsernamePassword && d.Username == email).FirstOrDefault();
            var filter = Builders<UserDocument>.Filter;
            var userdocauthdocfilter = filter.And(
                filter.Eq(x => x._id, userDocument._id),
                filter.ElemMatch(x => x.AuthenticationDocuments, c => c._id == authDocument._id)
            );
            var update = Builders<UserDocument>.Update;
            var activator = update.Set("AuthenticationDocuments.$.isActive", true);

            await userCollection.UpdateOneAsync(userdocauthdocfilter, activator);
        }

        public async Task<UserDocument> GetUserDocumentByUidAsync(string uid)
        {
            var userDocument = await userCollection.FindAsync(x => x.Uid == uid);
            var result = userDocument.FirstOrDefault();

            if (result == null)
            {
                throw new Exception("User not found");
            }
            else
            {
                return result;
            }
        }
    }

}