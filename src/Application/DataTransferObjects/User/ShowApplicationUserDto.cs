using Domain.Enums;
using System;

namespace Application.DataTransferObjects.User
{
    public class ShowApplicationUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
    }
}