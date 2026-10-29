using System;
using System.Collections.Generic;
using System.Text;

namespace HostelRoomManagement.Models;

public class Student
{
    public int StudentId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int? RoomId { get; set; }

    public Room? Room { get; set; }
}