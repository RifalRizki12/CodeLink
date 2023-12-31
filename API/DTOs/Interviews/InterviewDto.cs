﻿using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Interviews; 

public class InterviewDto //untuk update interview yang biasa
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string EmailIdle { get; set; }
    public DateTime Date { get; set; }
    public string Remarks { get; set; }
    public DateTime? StartContract { get; set; }
    public DateTime? EndContract { get; set; }
    public Guid EmployeeGuid { get; set; }
    public Guid OwnerGuid { get; set; }
    public string Interviewer { get; set; }
    public string Idle {  get; set; }
    public string StatusIdle {  get; set; }
    public TypeInterview? Type {  get; set; }
    public string Location { get; set; }
    public StatusIntervew? StatusInterview { get; set; }
    public int? rate { get; set; }
    public string feedback { get; set; }

    public static explicit operator InterviewDto(Employee employee)
    {
        return new InterviewDto
        {
            Interviewer = employee.FirstName + " " + employee.LastName,
            Idle = employee.FirstName + " " + employee.LastName,
            StatusIdle = employee.StatusEmployee.ToString(),

        };
    }

    public static explicit operator InterviewDto(Rating rating)
    {
        return new InterviewDto
        {
            rate = rating.Rate,
            feedback = rating.Feedback
        };
    }

    public static explicit operator InterviewDto(Interview interview)
    {
        return new InterviewDto
        {
            Guid = interview.Guid,
            Name = interview.Name,
            Date = interview.Date,
            Remarks = interview.Remarks,
            OwnerGuid = interview.OwnerGuid,
            StartContract = interview.StartContract,
            EndContract = interview.EndContract,
            EmployeeGuid = interview.EmployeeGuid,
            Type = interview.Type,
            Location = interview.Location,
            StatusInterview = interview.StatusIntervew
            
        };
    }

    public static implicit operator Interview(InterviewDto interviewDto)
    {
        return new Interview
        {
            Guid = interviewDto.Guid,
            Name = interviewDto.Name,
            Date = interviewDto.Date,
            Remarks = interviewDto.Remarks,
            StartContract = interviewDto.StartContract,
            EndContract = interviewDto.EndContract,
            OwnerGuid= interviewDto.OwnerGuid,
            EmployeeGuid = interviewDto.EmployeeGuid,
            ModifiedDate = DateTime.Now
        };
    }
}
