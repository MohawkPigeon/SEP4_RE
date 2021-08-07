using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using sep4;
using sep4.Models;

namespace sep4.Controllers
{
    public class APIUsersController : ApiController
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: api/APIUsers
        [ResponseType(typeof(List<UserDTO>))]
        public IHttpActionResult GetUsers()
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (var user in db.User.ToList())
            {
                UserDTO userDTO = new UserDTO(user.UserID, user.Username, user.Password, user.Rights);
                userDTOs.Add(userDTO);
            }

            return Ok(userDTOs);
        }

        // GET: api/APIUsers/5
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            UserDTO userDTO = new UserDTO(user.UserID, user.Username, user.Password, user.Rights);

            return Ok(userDTO);
        }

        // GET: api/APIUsers/username/password
        [Route("api/apiuser/{username}/{password}")]
        [ResponseType(typeof(UserDTO))]
        public IHttpActionResult GetUserWithLogin(String username, String password) 
        {
            User user = null;
            foreach (User userItem in db.User.ToList())
            {
                String name = userItem.Username.Trim();
                String pass = userItem.Password.Trim();
                if (name.Equals(username) && pass.Equals(password))
                {
                     user = db.User.Find(userItem.UserID);
                }
            }
            if (user == null)
            {
                return NotFound();
            }

            UserDTO userDTO = new UserDTO(user.UserID, user.Username, user.Password, user.Rights);

            return Ok(userDTO);
        }

        // PUT: api/APIUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            if (user.Rights == "Supervisor")
            {
                StageSupervisorDim stageSupervisor = db.StageSupervisorDim.Where(ss => ss.UserID == user.UserID && ss.ValidTo > DateTime.Now).FirstOrDefault();
                if (stageSupervisor != null)
                    stageSupervisor.ValidTo = DateTime.Now.AddDays(-1);
            }
            if (user.Rights == "User")
            {
                StageUserDim stageUser = db.StageUserDim.Where(su => su.UserID == user.UserID && su.ValidTo > DateTime.Now).FirstOrDefault();
                if (stageUser != null)
                    stageUser.ValidTo = DateTime.Now.AddDays(-1);
            }

            user.DateTime = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/APIUsers
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.DateTime = DateTime.Now;
            db.User.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/APIUsers/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            if(user.Rights == "Supervisor")
            {
                StageSupervisorDim stageSupervisor = db.StageSupervisorDim.Where(ss => ss.UserID == user.UserID && ss.ValidTo > DateTime.Now).FirstOrDefault();
                if(stageSupervisor != null)
                stageSupervisor.ValidTo = DateTime.Now.AddDays(-1);
            }
            if (user.Rights == "User")
            {
                StageUserDim stageUser = db.StageUserDim.Where(su => su.UserID == user.UserID && su.ValidTo > DateTime.Now).FirstOrDefault();
                if(stageUser != null)
                stageUser.ValidTo = DateTime.Now.AddDays(-1);
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.UserID == id) > 0;
        }
    }
}