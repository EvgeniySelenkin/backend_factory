using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson1
{
    interface IGetInfo
    {
        static int Id { get; set; }
        static string Name { get; set; }
        public void GetInformation();
    }
}
