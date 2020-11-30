# Scheduling Service
A service to very naively create "TimeSlots" in an imaginary calendar for a specific user.

## Required
[.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)

## Running the service
After cloning the repository the api can be run with 

`dotnet run --project src/Scheduling.WebApi/`

The default port is 5000 so the api will be running at http://localhost:5000 -- This can be changed via `src/Scheduling.WebApi/Properties.launchSettings.json`

### Types & properties
| Name | Description |
|--|--|
|startTimeStamp | Int64: based on [epoch timestamp](https://www.epochconverter.com/) in seconds |
|duration | Int: Time in minutes |
|userId | Int: Imaginary id all values over 0 |
|timeSlotId | Int: Id of TimeSlot in database |
|TimeSlot | An aggregation of the 4 previous properties

### Api Endpoints

| Method | Endpoint | Required Query Parameters | Response (JSON) |
|--|--|--|--|
| GET | /api/timeslot/ |  | `{[Array of TimeSlots]}`|
| GET | /api/timeslot/is-available | startTimeStamp, duration, userId | `{"isAvailable":"boolean value"}`
| POST | /api/timeslot/|startTimeStamp, duration, userId | `{"message": "...", "createdTimeSlot":"..timeslot data.."}`
| DELETE | /api/timeslot/ | startTimeStamp, duration, userId | `{"message":  "...","deletedTimeSlots":  [Array of TimeSlots]}`
| DELETE | /api/timeslot/{id:Integer}/ | |`{"message":  "...","deletedTimeSlots":  [Array of TimeSlots]}`


## Assumptions made
- Duration will be integer in minutes
- Any time is bookable
- Deleting a time slot range will delete any timeslot contained within the range including only start or end times of another timeslot
- Timeslots will have to be delegated to individual user ids
- Timestamps required will be unix epoch time in seconds
- Durations have a max of 24 hours
