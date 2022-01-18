using DAL.Model;
using EFLibCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersOperationsController : ControllerBase
    {
        List<User> userList = new List<User>();
        Result _result = new Result();

        DBOperations dbOperation = new DBOperations();
        /// <summary>
        /// Sistemdeki kullan�c� bilgilerini getirir.
        /// </summary>
        /// <returns> Kullan�c� listesini d�ner </returns>

        [Authorize]
        [HttpGet]
        public List<User> GetUser()
        {
           
            return dbOperation.GetUsers();
        }

        [HttpGet("/UsersOperations/GetUserPaging")]
        public IActionResult GetUserPaging([FromQuery] OwnerParameters ownerParameters)
        {
         var owners = dbOperation.GetUsers() //Db de users tablosundaki her�ey
         //.OrderBy(on => on.Name) //S�ralama
        .Skip(ownerParameters.PageNumber) //ka��nc� kay�ttn itibaren veri gelecek
        .Take(ownerParameters.PageSize) //Belirtti�imiz say�da kay�t getirir. �rn : 3, 2 
        .ToList();


            return Ok(owners);
        }

        /// <summary>
        /// Sistemdeki terk bir kullan�c�y� getirir.
        /// </summary>
        /// <param name="userId">�stenen kullan�c�n�n userId bilgisi</param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            List<User> userList = new List<User>();

            User? resultObject = new User();
            resultObject = userList.Find(x => x.Id == id);
            return resultObject;
           
        }

        /// <summary>
        /// Yeni bir kullan�c� ekler
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [HttpPost]
        public Result Post(User user)
        {
            User usr = dbOperation.FindUser(user.Name, user.UserName);
            //Yeni eleman listede var m� ? 
            bool userCheck = (usr != null) ?  true : false;
           
            if (userCheck == false)
            {
                //Listeye yeni eleman ekleniyor.
                if(dbOperation.AddModel(user) == true)
                {
                    _result.status = 1;
                    _result.Message = "Yeni eleman listeye eklendi.";
                }
                else
                {
                    _result.status = 0;
                    _result.Message = "Hata, kullan�c� eklenemedi.";
                }
                
            }
            else
            {
                _result.status = 0;
                _result.Message = "Bu eleman listede zaten var.";
            }
            
            return _result;
        }

        /// <summary>
        /// Kullan�c� g�nceller. 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        /// 
        
        [HttpPut("{UserId}")]
        public Result Update(int UserId, User newValue)
        {

            //Kullan�c� g�ncelleme i�lemi yap�l�r.
            User? _oldValue = userList.Find(o => o.Id == UserId);
            if (_oldValue != null)
            {
                userList.Add(newValue);
                userList.Remove(_oldValue);

                _result.status = 1;
                _result.Message = "Kullan�c� bilgileri ba�ar�yla g�ncellendi";
                _result.UserList = userList;
            }
            else
            {
                _result.status = 0;
                _result.Message = "Bu kullan�c�y� i�erde bulamad�k.";
            }
            return _result;

        }
        
        /// <summary>
        /// Kullan�c� sil
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete("{UserId}")]
        public Result Delete(int UserId)
        {
            if (dbOperation.DeleteModel(UserId))
            {
                _result.status = 1;
                _result.Message = "Kullan�c� silindi";
                _result.UserList = userList;
            }
            else
            {
                _result.status = 0;
                _result.Message = "Kullan�c� zaten silinmi�ti.";
            }
            return _result;
        }

    }
}