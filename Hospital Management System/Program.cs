using Hospital_Management_System.Models;
using System.Text.RegularExpressions;

namespace Hospital_Management_System
{
    public class Program
    {

        // 1 Register Patient
        //*************************************************************//

        public static void RegisterPatient(HospitalContext context)
        {
            Console.WriteLine("\n=== Register New Patient ===");

            Console.Write("Enter Patient Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Patient Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Enter Patient Gender: ");
            string gender = Console.ReadLine();

            Console.Write("Enter Patient Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Enter Patient Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Blood Type: ");
            string bloodType = Console.ReadLine();


            int patientId = context.Patients.Count + 1;


            context.Patients.Add(
                new Patient(
                    patientId,
                    name,
                    age,
                    gender,
                    phone,
                    email,
                    bloodType
                )
            );


            Console.WriteLine(
                $"Patient registered successfully. ID: {patientId}"
            );
        }



        // 2 Add Doctor
        //*************************************************************//

        public static void AddNewDoctor(HospitalContext context)
        {
            Console.WriteLine("\n=== Add New Doctor ===");


            Console.Write("Doctor Name: ");
            string name = Console.ReadLine();


            Console.Write("Specialization: ");
            string specialization = Console.ReadLine();


            Console.Write("Phone: ");
            string phone = Console.ReadLine();


            Console.Write("Email: ");
            string email = Console.ReadLine();


            Console.Write("Consultation Fee: ");
            decimal fee = decimal.Parse(Console.ReadLine());


            int doctorId = context.Doctors.Count + 1;


            context.Doctors.Add(new Doctor
            {
                doctorId = doctorId,
                doctorName = name,
                doctorSpecialization = specialization,
                doctorPhone = phone,
                doctorEmail = email,
                consultationFee = fee
            });



            Console.WriteLine(
                $"Doctor added successfully. ID: {doctorId}"
            );
        }




        // 3 View Patients
        //*************************************************************//

        public static void ViewAllPatients(HospitalContext context)
        {

            Console.WriteLine("\n=== All Patients ===");


            foreach (Patient p in context.Patients)
            {
                Console.WriteLine($"ID: {p.patiendId}  |  Name: {p.patientName}  |  Age: {p.patientAge}" +
                                  $"  |  Gender: {p.patientGender}  |  Blood Type: {p.patientBloodType}" +
                                  $"  |  Phone: {p.patientPhone}  |  Email: {p.patientEmail}");
            }

        }





        // 4 View Doctors By Specialization
        //*************************************************************//

        public static void ViewAllDoctorsBySpecialization(HospitalContext context)
        {
            Console.WriteLine("\n=== Search Doctors by Specialization ===");


            Console.Write("Enter Specialization: ");
            string specialization = Console.ReadLine();



            List<Doctor> doctors =
                context.Doctors
                .Where(d =>
                d.doctorSpecialization.ToLower()
                ==
                specialization.ToLower())
                .ToList();



            foreach (Doctor d in doctors)
            {
                Console.WriteLine($"ID: {d.doctorId}  |  Name: {d.doctorName}" +
                                  $"  |  Phone: {d.doctorPhone}  |  Fee: {d.consultationFee:C}");
            }




        }





        // 5 Add Slot
        //*************************************************************//

        public static void AddAvailableSlot(HospitalContext context)
        {
            Console.WriteLine("\n=== Add Available Slot for Doctor ===");

            foreach (Doctor d in context.Doctors)
            {
                Console.WriteLine($"  ID: {d.doctorId}  |  {d.doctorName}  ({d.doctorSpecialization})");
            }

            Console.Write("Enter Doctor ID: ");

            int doctorId =int.Parse(Console.ReadLine());

            Doctor doctor =
                context.Doctors
                .FirstOrDefault(d =>
                d.doctorId == doctorId);


            if (doctor == null)
            {
                Console.WriteLine("Doctor not found");
                return;
            }

            bool result = context.Doctors.Any(d => d.doctorId == doctorId);
            if (result == false)
            {
                Console.WriteLine("doctor not found please insert a correct Id");
                return;
            }

            Console.Write("Slot Date: ");
            string date = Console.ReadLine();

            Console.Write("Slot Time: ");
            string time = Console.ReadLine();

            int slotId = context.AvailableSlots.Count + 1;


            context.AvailableSlots.Add(
                new AvailableSlot
                {
                    slotId = slotId,
                    doctorId = doctorId,
                    slotDate = date,
                    slotTime = time,
                    isBooked = false
                });



            Console.WriteLine($"Slot added successfully with slotId: {slotId}");
        }

        // 6 Book Appointment
        //*************************************************************//

        public static void BookAppointment(HospitalContext context)
        {
            Console.WriteLine("\n=== Book Appointment ===");


            Console.Write("Enter Patient ID: ");
            int patientId = int.Parse(Console.ReadLine());


            Patient patient =
                context.Patients
                .FirstOrDefault(p => p.patiendId == patientId);


            if (patient == null)
            {
                Console.WriteLine("Patient not found");
                return;
            }



            Console.Write("Enter Doctor ID: ");
            int doctorId = int.Parse(Console.ReadLine());


            Doctor doctor =
                context.Doctors
                .FirstOrDefault(d => d.doctorId == doctorId);



            if (doctor == null)
            {
                Console.WriteLine("Doctor not found");
                return;
            }



            List<AvailableSlot> slots =
                context.AvailableSlots
                .Where(s =>
                    s.doctorId == doctorId &&
                    s.isBooked == false)
                .ToList();



            if (!slots.Any())
            {
                Console.WriteLine("No available slots");
                return;
            }



            foreach (AvailableSlot s in slots)
            {
                Console.WriteLine(
                $"Slot ID: {s.slotId} Date: {s.slotDate} Time: {s.slotTime}");
            }



            Console.Write("Choose Slot ID: ");
            int slotId = int.Parse(Console.ReadLine());



            AvailableSlot selectedSlot =
                slots.FirstOrDefault(s =>
                s.slotId == slotId);



            if (selectedSlot == null)
            {
                Console.WriteLine("Invalid slot");
                return;
            }




            Appointment appointment =
                new Appointment
                {
                    appointmentId =
                    context.Appointments.Count + 1,

                    patientId = patientId,

                    doctorId = doctorId,

                    appointmentDate =
                    selectedSlot.slotDate,

                    appointmentTime =
                    selectedSlot.slotTime,

                    status = "Scheduled"
                };



            context.Appointments.Add(appointment);



            selectedSlot.isBooked = true;



            Console.WriteLine(
            $"Appointment booked successfully. ID: {appointment.appointmentId}");

        }






        // 7 Cancel Appointment
        //*************************************************************//

        public static void CancelAppointment(HospitalContext context)
        {

            Console.Write("Enter Appointment ID: ");

            int appointmentId =
                int.Parse(Console.ReadLine());



            Appointment appointment =
                context.Appointments
                .FirstOrDefault(a =>
                a.appointmentId == appointmentId);



            if (appointment == null)
            {
                Console.WriteLine("Appointment not found");
                return;
            }



            if (appointment.status == "Cancelled")
            {
                Console.WriteLine("Already cancelled");
                return;
            }



            appointment.status = "Cancelled";



            AvailableSlot slot =
                context.AvailableSlots
                .FirstOrDefault(s =>
                    s.doctorId == appointment.doctorId &&
                    s.slotDate == appointment.appointmentDate &&
                    s.slotTime == appointment.appointmentTime);



            if (slot != null)
            {
                slot.isBooked = false;
            }



            Console.WriteLine(
            "Appointment cancelled successfully");

        }








        // 8 Create Medical Record
        //*************************************************************//

        public static void CreateMedicalRecord(HospitalContext context)
        {

            Console.Write("Enter Appointment ID: ");

            int appointmentId =
                int.Parse(Console.ReadLine());



            Appointment appointment =
                context.Appointments
                .FirstOrDefault(a =>
                a.appointmentId == appointmentId);



            if (appointment == null)
            {
                Console.WriteLine("Appointment not found");
                return;
            }



            if (appointment.status == "Cancelled")
            {
                Console.WriteLine("Cancelled appointment");
                return;
            }



            Console.Write("Diagnosis: ");
            string diagnosis = Console.ReadLine();



            Console.Write("Prescription: ");
            string prescription = Console.ReadLine();



            Doctor doctor =
                context.Doctors
                .FirstOrDefault(d =>
                d.doctorId == appointment.doctorId);



            MedicalRecord record =
                new MedicalRecord
                {
                    recordId =
                    context.MedicalRecords.Count + 1,


                    patientId =
                    appointment.patientId,


                    doctorId =
                    appointment.doctorId,


                    appointmentId =
                    appointmentId,


                    diagnosis = diagnosis,


                    prescription = prescription,


                    visitDate =
                    DateTime.Now.ToString(),


                    visitFee =
                    doctor.consultationFee

                };



            context.MedicalRecords.Add(record);



            appointment.status = "Completed";



            Console.WriteLine(
            "Medical record created successfully");

        }







        // 9 Patient History
        //*************************************************************//

        public static void GeneratePatientMedicalHistory(
            HospitalContext context)
        {

            Console.Write("Enter Patient ID: ");

            int patientId =
                int.Parse(Console.ReadLine());



            Patient patient =
                context.Patients
                .FirstOrDefault(p =>
                p.patiendId == patientId);



            if (patient == null)
            {
                Console.WriteLine("Patient not found");
                return;
            }



            List<MedicalRecord> records =
                context.MedicalRecords
                .Where(r =>
                r.patientId == patientId)
                .ToList();



            if (!records.Any())
            {
                Console.WriteLine("No records");
                return;
            }



            Console.WriteLine(
            $"History for {patient.patientName}");



            decimal total = 0;



            foreach (var r in records)
            {

                Doctor doctor =
                    context.Doctors
                    .FirstOrDefault(d =>
                    d.doctorId == r.doctorId);



                Console.WriteLine("----------------");

                Console.WriteLine(
                $"Doctor: {doctor.doctorName}");

                Console.WriteLine(
                $"Diagnosis: {r.diagnosis}");

                Console.WriteLine(
                $"Prescription: {r.prescription}");

                Console.WriteLine(
                $"Fee: {r.visitFee}");



                total += r.visitFee;
            }



            Console.WriteLine(
            $"Total Charges: {total}");

        }

        // 10 Doctor Revenue Summary
        //*************************************************************//
        public static void GenerateDoctorWorkloadReport(
            HospitalContext context)
        {

            Console.WriteLine("\n=== Doctor Workload & Revenue Summary ===");



            var report =
                context.Doctors
                .Select(d => new
                {

                    DoctorName = d.doctorName,


                    Completed =
                    context.Appointments.Count(a =>
                    a.doctorId == d.doctorId &&
                    a.status == "Completed"),



                    Cancelled =
                    context.Appointments.Count(a =>
                    a.doctorId == d.doctorId &&
                    a.status == "Cancelled"),




                    Revenue =
                    context.MedicalRecords
                    .Where(r =>
                    r.doctorId == d.doctorId)
                    .Sum(r =>
                    (decimal?)r.visitFee) ?? 0


                })
                .OrderByDescending(x => x.Revenue)
                .ToList();



            if (!report.Any())
            {
                Console.WriteLine("No data found");
                return;
            }



            Console.WriteLine(
            "\nDoctor\tCompleted\tCancelled\tRevenue");



            foreach (var item in report)
            {

                Console.WriteLine(
                $"{item.DoctorName}\t" +
                $"{item.Completed}\t\t" +
                $"{item.Cancelled}\t\t" +
                $"{item.Revenue}");

            }

        }


        // 11 — Search Patient By Name
        //*************************************************************//
        public static void SearchPatientByName(HospitalContext context)
        {
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine().ToLower();

            List<Patient> patients = context.Patients
                .Where(p => p.patientName.ToLower().Contains(name))
                .ToList();

            if (!patients.Any())
            {
                Console.WriteLine("No patient found.");
                return;
            }

            foreach (Patient patient in patients)
            {
                Console.WriteLine(
                    $"ID: {patient.patiendId} | Name: {patient.patientName}");
            }
        }



        // 12 —  DailyAppointmentReport
        //*************************************************************//

        public static void DailyAppointmentReport(HospitalContext context)
        {
            Console.Write("Enter date: ");
            string date = Console.ReadLine();

            var report =
                context.Appointments
                .Where(a => a.appointmentDate == date)
                .Select(a => new
                {
                    AppointmentId = a.appointmentId,

                    PatientName = context.Patients
                        .FirstOrDefault(p => p.patiendId == a.patientId)?
                        .patientName,

                    DoctorName = context.Doctors
                        .FirstOrDefault(d => d.doctorId == a.doctorId)?
                        .doctorName,

                    Status = a.status
                })
                .ToList();

            if (!report.Any())
            {
                Console.WriteLine("No appointments found.");
                return;
            }

            foreach (var item in report)
            {
                Console.WriteLine(
                    $"Appointment: {item.AppointmentId} | " +
                    $"Patient: {item.PatientName} | " +
                    $"Doctor: {item.DoctorName} | " +
                    $"Status: {item.Status}");
            }
        }

        // 13 — MostActivePatientReport
        //*************************************************************//

        public static void MostActivePatientReport(HospitalContext context)
        {
            Console.WriteLine("\n=== Most Active Patient Report ===");

            if (!context.Appointments.Any())
            {
                Console.WriteLine("No appointments found.");
                return;
            }

            var report = context.Appointments
                .GroupBy(a => a.patientId)
                .Select(g => new
                {
                    PatientId = g.Key,

                    TotalAppointments = g.Count(),

                    CompletedAppointments =
                        g.Count(a => a.status == "Completed"),

                    CancelledAppointments =
                        g.Count(a => a.status == "Cancelled")
                })
                .OrderByDescending(x => x.TotalAppointments)
                .FirstOrDefault();



            Patient patient = context.Patients
                .FirstOrDefault(p => p.patiendId == report.PatientId);



            Console.WriteLine($"Patient ID          : {patient.patiendId}");
            Console.WriteLine($"Patient Name        : {patient.patientName}");
            Console.WriteLine($"Total Appointments  : {report.TotalAppointments}");
            Console.WriteLine($"Completed Visits    : {report.CompletedAppointments}");
            Console.WriteLine($"Cancelled Visits    : {report.CancelledAppointments}");
        }

        //***********************************************************************************//
        public static void Main(string[] args)
        {
            //data storge for the system (in memory)
            HospitalContext maincontext = new HospitalContext();
            maincontext.Patients = new List<Patient>();
            maincontext.Doctors = new List<Doctor>();
            maincontext. Appointments = new List<Appointment>();
            maincontext. AvailableSlots = new List<AvailableSlot>();
            maincontext. MedicalRecords = new List<MedicalRecord>();


            bool exit = false;
            while(exit == false) 
            {
                //let the system begin:
                Console.WriteLine("****************************************");
                Console.WriteLine("Welcom to the Hospital Management System ");
                Console.WriteLine("****************************************");
                Console.WriteLine("1. Register Patient");
                Console.WriteLine("2. Add NEW Doctor");
                Console.WriteLine("3. View All Patients");
                Console.WriteLine("4. View All Doctors By Specialization");
                Console.WriteLine("5. Add Available Time Slot");
                Console.WriteLine("6. Book Appointment");
                Console.WriteLine("7. Cancel Appointment");
                Console.WriteLine("8. Create Medical Record");
                Console.WriteLine("9. Patient History Report");
                Console.WriteLine("10. Doctor Revenue Summary");
                Console.WriteLine("11. SearchPatientByName");
                Console.WriteLine("12. Daily Appointment Report");
                Console.WriteLine("13. Most Active Patient Report");
                Console.WriteLine("0. Exit");
                Console.WriteLine("****************************************");
                Console.WriteLine("Choose : ");



                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        RegisterPatient(maincontext);
                        break;

                    case 2:
                        AddNewDoctor(maincontext);
                        break;

                    case 3:
                        ViewAllPatients(maincontext);
                        break;

                    case 4:
                        ViewAllDoctorsBySpecialization(maincontext);
                        break;

                    case 5:
                        AddAvailableSlot(maincontext);
                        break;

                    case 6:
                        BookAppointment(maincontext);
                        break;

                    case 7:
                        CancelAppointment(maincontext);
                        break;

                    case 8:
                        CreateMedicalRecord(maincontext);
                        break;

                    case 9:
                        GeneratePatientMedicalHistory(maincontext);
                        break;

                    case 10:
                        GenerateDoctorWorkloadReport(maincontext);
                        break;

                    case 11:
                        SearchPatientByName(maincontext);
                        break;

                    case 12:
                        DailyAppointmentReport(maincontext);
                        break;

                    case 13:
                        MostActivePatientReport(maincontext);
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.Please try again.");
                        break;
                }
                Console.WriteLine("Press any key to continue....");
                Console.ReadLine();
                Console.Clear();
            }

        }
    }
}
