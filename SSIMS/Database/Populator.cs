using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.Database
{
    public class Populator
    {
        static void InitStaff(DatabaseContext context)
        {
            List<Staff> staffs = new List<Staff>
            {
                //Architecture Department Dept 1
                new Staff("Prof. Kian Bong Kee","81210001","kianbongkee@u.logic.edu",1,"DeptHead"),
                new Staff("Mr. Wan Lau Eh", "91211002","wanlaueh@u.logic.edu",1,"DeptRep"),
                new Staff("Mr. Timmy Ting","81213003","timmyting@u.logic.edu",1,"Staff"),
                new Staff("Mr. Patrick Coy","81215005","patrickcoy@u.logic.edu",1,"Staff"),
                new Staff("Mr. Ye Yint Hein", "91214004","yeyinthein@u.logic.edu",1,"Staff"),
                new Staff("Mrs. Amanda Ceb","81210601","amandaceb@u.logic.edu",1,"Staff"),
                new Staff("Mrs. Sakura Shinji", "91210009","sakurashinji@u.logic.edu",1,"Staff"),

                //Arts Department Dept 2
                new Staff("Prof. Su Myat Mon", "91710001","sumyatmon@u.logic.edu",2,"DeptHead"),
                new Staff("Mrs. Sumi Ko","81711002","sumiko@u.logic.edu",2,"DeptRep"),
                new Staff("Mr. Kendrik Carlo", "91713003","kendricarlo@u.logic.edu",2,"Staff"),
                new Staff("Mr. Kenny Tim","81715005","timkenny@u.logic.edu",2,"Staff"),
                new Staff("Mr. Timmy Pong", "91714004","timmypong@u.logic.edu",2,"Staff"),
                new Staff("Mrs. Yod Pornpattrison","81710601","yodpattrison@u.logic.edu",2,"Staff"),
                new Staff("Mrs. Kabuto Sumiya", "91710009","sumiyakabuto@u.logic.edu",2,"Staff"),

                //Commerce Department Dept 3
                new Staff("Dr. Chiah Leow Bee", "98910001","chiahleowbee@u.logic.edu",3,"DeptHead"),
                new Staff("Mr. Mohd Azman","88911002","mohdazman@u.logic.edu",3,"DeptRep"),
                new Staff("Mrs. Tammy Berth", "98913003","tammyberth@u.logic.edu",3,"Staff"),
                new Staff("Mrs. Summer Tran", "88915005","summertran@u.logic.edu",3,"Staff"),
                new Staff("Mr. John Tang", "98914004","johntang@u.logic.edu",3,"Staff"),
                new Staff("Mr. Jones Fong", "88910601","jonesfong@u.logic.edu",3,"Staff"),
                new Staff("Mrs. Rebecca Hong", "98910009","rebeccahong@u.logic.edu",3,"Staff"),

                //Computer Science Department Dept 4
                new Staff("Dr. Soh Kian Wee", "89010001","sohkianwee@u.logic.edu",4,"DeptHead"),
                new Staff("Mr. Week Kian Fatt", "99011002","weekianfat@u.logic.edu",4,"DeptRep"),
                new Staff("Mr. Dan Shiok", "99013003","danshiok@u.logic.edu",4,"Staff"),
                new Staff("Mr. Andrew Lee","89015005","andrewlee@u.logic.edu",4,"Staff"),
                new Staff("Mr. Kaung Kyaw", "99014004","kaungkyaw@u.logic.edu",4,"Staff"),
                new Staff("Mrs. Lina Lim", "89010601","linalim@u.logic.edu",4,"Staff"),
                new Staff("Mrs. Temari Ang", "99010009","temariang@u.logic.edu",4,"Staff"),

                //Engineering Department Dept 5
                new Staff("Prof. Lucas Liang Tan", "98110001","lucasliangtan@u.logic.edu",5,"DeptHead"),
                new Staff("Mr. Yoshi Nori","88111002","yoshinori@u.logic.edu",5,"DeptRep"),
                new Staff("Mr. Ron Kent", "98113003","ronkent@u.logic.edu",5,"Staff"),
                new Staff("Mr. Kim Jung Ho", "98115005","kimjungho@u.logic.edu",5,"Staff"),
                new Staff("Mr. Michael Angelo","88114004","michaelangelo@u.logic.edu",5,"Staff"),
                new Staff("Mrs. Sandra Cooper","88110601","sandracooper@u.logic.edu",5,"Staff"),
                new Staff("Mrs. Jennifer Bullock", "98110009","jenniferbullock@u.logic.edu",5,"Staff"),

                //English Department Dept 6
                new Staff("Prof. Ezra Pound", "90010001","ezrapound@u.logic.edu",6,"DeptHead"),
                new Staff("Mrs. Pamela Kow","80011002","pamelakow@u.logic.edu",6,"DeptRep"),
                new Staff("Mr. Jacob Duke", "90013003","jacobduke@u.logic.edu",6,"Staff"),
                new Staff("Mrs. Andrea Linux","80015005","andrelinux@u.logic.edu",6,"Staff"),
                new Staff("Mrs. Anne Low", "90014004","annelow@u.logic.edu",6,"Staff"),
                new Staff("Mrs. May Tan","82010601","maytan@u.logic.edu",6,"Staff"),
                new Staff("Mrs. June Nguyen", "91010009","junenguyen@u.logic.edu",6,"Staff"),

                //Medicine Department Dept 7
                new Staff("Prof. Russel Jones","81110001","russeljones@u.logic.edu",7,"DeptHead"),
                new Staff("Mrs. Jin Chia Lin","81111002","jinchialin@u.logic.edu",7,"DeptRep"),
                new Staff("Mr. Duke Joneson","81113003","dukejoneson@u.logic.edu",7,"Staff"),
                new Staff("Mrs. Andrea Hei", "91115005","andreahei@u.logic.edu",7,"Staff"),
                new Staff("Mrs. Wendy Loo", "91114004","wendyloo@u.logic.edu",7,"Staff"),
                new Staff("Mrs. July Moh", "91110601","julymoh@u.logic.edu",7,"Staff"),
                new Staff("Mr. Augustus Robinson", "91110009","augustusrobinson@u.logic.edu",7,"Staff"),

                //Registrar Department Dept 8
                new Staff("Mrs. Low Kway Boo", "95610001","lowkwayboo@u.logic.edu",8,"DeptHead"),
                new Staff("Ms. Helen Ho", "95611002","helenho@u.logic.edu",8,"DeptRep"),
                new Staff("Mr. Ngoc Thuy", "95613003","ngocthuy@u.logic.edu",8,"Staff"),
                new Staff("Mrs. Chan Chen", "95615005","chanchen@u.logic.edu",8,"Staff"),
                new Staff("Mr. Tommy Lee Johnson","85614004","tomleejohnson@u.logic.edu",8,"Staff"),
                new Staff("Mr. Toni Than","85610601","tonithan@u.logic.edu",8,"Staff"),
                new Staff("Mrs. Tra Xiang","85610009","traxiang@u.logic.edu",8,"Staff"),

                //Science Department Dept 9
                new Staff("Mrs. Polly Timberland", "95890001","pollytimberland@u.logic.edu",9,"DeptHead"),
                new Staff("Ms. Penny Shelby", "95891002","pennyshelby@u.logic.edu",9,"DeptRep"),
                new Staff("Mr. Thomas Thompson", "956893003","thomasthompson@u.logic.edu",9,"Staff"),
                new Staff("Mrs. Alice Yu", "95895005","aliceyu@u.logic.edu",9,"Staff"),
                new Staff("Mr. Victor Tun", "95894004","victortun@u.logic.edu",9,"Staff"),
                new Staff("Mr. Stanley Presley", "95890601","stanleypresley@u.logic.edu",9,"Staff"),
                new Staff("Mrs. Pan Tan", "95890009","pantan@u.logic.edu",9,"Staff"),

                //Zoology Department Dept 10
                new Staff("Prof. Tan", "94810001","tanzoo67@u.logic.edu",10,"DeptHead"),
                new Staff("Mr. Peter Tan Ah Meng", "94811002","petertan@u.logic.edu",10,"DeptRep"),
                new Staff("Mrs. Ching Chen", "94813003","chingchen@u.logic.edu",10,"Staff"),
                new Staff("Mrs. Natalie Noel", "94815005","noelnatalie@u.logic.edu",10,"Staff"),
                new Staff("Mr. Donald Dom", "94814004","domdonald@u.logic.edu",10,"Staff"),
                new Staff("Mr. Ting Xiong", "94810601","tingxiong@u.logic.edu",10,"Staff"),
                new Staff("Mrs. Jiang Huang", "94810009","jianghuang@u.logic.edu",10,"Staff"),
                new Staff()


            };
            foreach (Staff staff in staffs)
                context.Staffs.Add(staff);
            context.SaveChanges();
        }
    }
}