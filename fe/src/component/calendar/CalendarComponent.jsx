import { useEffect } from "react";
import { useState } from "react";
import API from "../../api/axios";
import { GET_USER_TASK } from "../../api/url";
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';

// const tasks = [
//     // Your task data here
//     {
//       id: 0,
//       taskPriorityId: 0,
//       taskStatusId: 0,
//       name: 'Task 1',
//       dateCreated: '2023-06-18T11:12:58.809Z',
//       dateScheduled: '2023-06-18T11:12:58.809Z',
//       parentId: 0,
//       description: 'Description of Task 1',
//       teamMembersUserIds: [0],
//       teamId: 0
//     },
//     {
//       id: 1,
//       taskPriorityId: 1,
//       taskStatusId: 1,
//       name: 'Task 2',
//       dateCreated: '2023-06-19T11:12:58.809Z',
//       dateScheduled: '2023-06-19T11:12:58.809Z',
//       parentId: 0,
//       description: 'Description of Task 2',
//       teamMembersUserIds: [0],
//       teamId: 0
//     },
//     // Add more tasks if needed
//   ];

export default function CalendarComponent() {
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [tasks, setTasks] = useState([]);

  useEffect(() => {
    API.get(`${GET_USER_TASK}${10}`)
      .then((response) => {
        console.log(response.data.data);
        setTasks(response.data.data);
      })
      .catch((erorr) => {
        if (erorr.response.status === 400) {
        }
      });
  }, []);

  const onChange = (date) => {
    setSelectedDate(date);
  };

  // Filter tasks for the selected date
  const filteredTasks = tasks.filter(
    (task) =>
      new Date(task.dateScheduled).toLocaleDateString() ===
      selectedDate.toLocaleDateString()
  );

  return (
    <div style={{display: "flex"}}>
      <div>
      <h2>Task Calendar</h2>
      <Calendar onChange={onChange} value={selectedDate} />
      </div>
      <div style={{paddingLeft: '30px',}}>
      <h3>Tasks for {selectedDate.toLocaleDateString()}</h3>
      {filteredTasks.length > 0 ? (
        <ul>
          {filteredTasks.map((task) => (
            <li key={task.id}>{task.name}</li>
          ))}
        </ul>
      ) : (
        <p>No tasks scheduled for this date.</p>
      )}
      </div>
    </div>
  );
}