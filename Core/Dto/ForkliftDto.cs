using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Dto
{
    public class ForkliftDto
    {
        public string Name { get; set; }
        public HttpPostedFileWrapper PictureWrapper { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
     //   public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }

        public List<Forklift> ListForklifts { get; set; }
        public List<ForkliftsModel> ListForkliftsModel { get; set; }
        public ForkliftsModel ForkliftsModel { get; set; }
    }
}
