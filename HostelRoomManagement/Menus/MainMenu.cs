using HostelRoomManagement.Services.Interfaces;

namespace HostelRoomManagement.Menus;

public class MainMenu
{
    private readonly IRoomService _roomService;
    private readonly IStudentService _studentService;
public MainMenu(
    IRoomService roomService,
    IStudentService studentService)
    {
        _roomService = roomService;
        _studentService = studentService;
    }

    public async Task RunAsync()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("       HOSTEL ROOM MANAGEMENT SYSTEM");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Manage Rooms");
            Console.WriteLine("2. Manage Students");
            Console.WriteLine("3. Allocate Room");
            Console.WriteLine("4. Vacate Room");
            Console.WriteLine("5. Exit");
            Console.WriteLine("========================================");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await ManageRoomsAsync();
                        break;

                    case "2":
                        await ManageStudentsAsync();
                        break;

                    case "3":
                        await AllocateRoomAsync();
                        break;

                    case "4":
                        await VacateRoomAsync();
                        break;

                    case "5":
                        exit = true;
                        Console.WriteLine("Thank you for using the system!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task ManageRoomsAsync()
    {
        Console.WriteLine("\n--- Manage Rooms ---");
        Console.WriteLine("1. View All Rooms");
        Console.WriteLine("2. View Room by ID");
        Console.WriteLine("3. Add Room");
        Console.WriteLine("4. Update Room");
        Console.WriteLine("5. Delete Room");
        Console.Write("Enter your choice: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var rooms = await _roomService.GetAllRoomsAsync();

                if (rooms.Count == 0)
                {
                    Console.WriteLine("No rooms found.");
                    return;
                }

                foreach (var room in rooms)
                {
                    Console.WriteLine(
                        $"ID: {room.RoomId} | " +
                        $"Room: {room.RoomNumber} | " +
                        $"Capacity: {room.Capacity} | " +
                        $"Occupied: {room.OccupiedBeds}");
                }

                break;

            case "2":
                Console.Write("Enter Room ID: ");

                if (!int.TryParse(Console.ReadLine(), out int roomId))
                {
                    Console.WriteLine("Invalid Room ID.");
                    return;
                }

                var selectedRoom =
                    await _roomService.GetRoomByIdAsync(roomId);

                if (selectedRoom is null)
                {
                    Console.WriteLine("Room not found.");
                    return;
                }

                Console.WriteLine($"Room ID: {selectedRoom.RoomId}");
                Console.WriteLine($"Room Number: {selectedRoom.RoomNumber}");
                Console.WriteLine($"Capacity: {selectedRoom.Capacity}");
                Console.WriteLine($"Occupied Beds: {selectedRoom.OccupiedBeds}");

                break;

            case "3":
                Console.Write("Enter Room Number: ");
                string roomNumber = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Capacity: ");

                if (!int.TryParse(Console.ReadLine(), out int capacity))
                {
                    Console.WriteLine("Invalid capacity.");
                    return;
                }

                var newRoom = new Models.Room
                {
                    RoomNumber = roomNumber,
                    Capacity = capacity,
                    OccupiedBeds = 0
                };

                await _roomService.AddRoomAsync(newRoom);

                Console.WriteLine("Room added successfully.");
                break;

            case "4":
                Console.Write("Enter Room ID: ");

                if (!int.TryParse(Console.ReadLine(), out int updateRoomId))
                {
                    Console.WriteLine("Invalid Room ID.");
                    return;
                }

                Console.Write("Enter New Room Number: ");
                string updatedRoomNumber = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter New Capacity: ");

                if (!int.TryParse(Console.ReadLine(), out int updatedCapacity))
                {
                    Console.WriteLine("Invalid capacity.");
                    return;
                }

                var roomToUpdate = new Models.Room
                {
                    RoomId = updateRoomId,
                    RoomNumber = updatedRoomNumber,
                    Capacity = updatedCapacity
                };

                await _roomService.UpdateRoomAsync(roomToUpdate);

                Console.WriteLine("Room updated successfully.");
                break;

            case "5":
                Console.Write("Enter Room ID to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int deleteRoomId))
                {
                    Console.WriteLine("Invalid Room ID.");
                    return;
                }

                await _roomService.DeleteRoomAsync(deleteRoomId);

                Console.WriteLine("Room deleted successfully.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    private async Task ManageStudentsAsync()
    {
        Console.WriteLine("\n--- Manage Students ---");
        Console.WriteLine("1. View All Students");
        Console.WriteLine("2. View Student by ID");
        Console.WriteLine("3. Add Student");
        Console.WriteLine("4. Update Student");
        Console.WriteLine("5. Delete Student");
        Console.Write("Enter your choice: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var students = await _studentService.GetAllStudentsAsync();

                if (students.Count == 0)
                {
                    Console.WriteLine("No students found.");
                    return;
                }

                foreach (var student in students)
                {
                    Console.WriteLine(
                        $"ID: {student.StudentId} | " +
                        $"Name: {student.FullName} | " +
                        $"Registration: {student.RegistrationNumber} | " +
                        $"Phone: {student.PhoneNumber} | " +
                        $"Room ID: {student.RoomId?.ToString() ?? "Not allocated"}");
                }

                break;

            case "2":
                Console.Write("Enter Student ID: ");

                if (!int.TryParse(Console.ReadLine(), out int studentId))
                {
                    Console.WriteLine("Invalid Student ID.");
                    return;
                }

                var selectedStudent =
                    await _studentService.GetStudentByIdAsync(studentId);

                if (selectedStudent is null)
                {
                    Console.WriteLine("Student not found.");
                    return;
                }

                Console.WriteLine($"Student ID: {selectedStudent.StudentId}");
                Console.WriteLine($"Name: {selectedStudent.FullName}");
                Console.WriteLine(
                    $"Registration Number: {selectedStudent.RegistrationNumber}");
                Console.WriteLine($"Phone: {selectedStudent.PhoneNumber}");
                Console.WriteLine(
                    $"Room ID: {selectedStudent.RoomId?.ToString() ?? "Not allocated"}");

                break;

            case "3":
                Console.Write("Enter Student Name: ");
                string fullName = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Registration Number: ");
                string registrationNumber = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Phone Number: ");
                string phoneNumber = Console.ReadLine() ?? string.Empty;

                var newStudent = new Models.Student
                {
                    FullName = fullName,
                    RegistrationNumber = registrationNumber,
                    PhoneNumber = phoneNumber
                };

                await _studentService.AddStudentAsync(newStudent);

                Console.WriteLine("Student added successfully.");
                break;

            case "4":
                Console.Write("Enter Student ID: ");

                if (!int.TryParse(Console.ReadLine(), out int updateStudentId))
                {
                    Console.WriteLine("Invalid Student ID.");
                    return;
                }

                Console.Write("Enter Updated Name: ");
                string updatedName = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Updated Registration Number: ");
                string updatedRegistration = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Updated Phone Number: ");
                string updatedPhone = Console.ReadLine() ?? string.Empty;

                var studentToUpdate = new Models.Student
                {
                    StudentId = updateStudentId,
                    FullName = updatedName,
                    RegistrationNumber = updatedRegistration,
                    PhoneNumber = updatedPhone
                };

                await _studentService.UpdateStudentAsync(studentToUpdate);

                Console.WriteLine("Student updated successfully.");
                break;

            case "5":
                Console.Write("Enter Student ID to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int deleteStudentId))
                {
                    Console.WriteLine("Invalid Student ID.");
                    return;
                }

                await _studentService.DeleteStudentAsync(deleteStudentId);

                Console.WriteLine("Student deleted successfully.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    private async Task AllocateRoomAsync()
    {
        Console.Write("Enter Student ID: ");

        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Invalid Student ID.");
            return;
        }

        Console.Write("Enter Room ID: ");

        if (!int.TryParse(Console.ReadLine(), out int roomId))
        {
            Console.WriteLine("Invalid Room ID.");
            return;
        }

        await _studentService.AllocateRoomAsync(studentId, roomId);

        Console.WriteLine("Room allocated successfully.");
    }

    private async Task VacateRoomAsync()
    {
        Console.Write("Enter Student ID: ");

        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Invalid Student ID.");
            return;
        }

        await _studentService.VacateRoomAsync(studentId);

        Console.WriteLine("Room vacated successfully.");
    }
}

