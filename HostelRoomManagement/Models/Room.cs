using System;
using System.Collections.Generic;
using System.Text;

namespace HostelRoomManagement.Models;

public class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public int OccupiedBeds { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();
}