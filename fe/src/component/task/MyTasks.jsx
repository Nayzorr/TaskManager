import API from "../../api/axios";
import { GET_USER_TASK } from "../../api/url";
import { useEffect, useState } from "react";
import { DataGrid } from "@mui/x-data-grid";

const columns = [
  { field: "name", headerName: "Name", flex: 1 },
  { field: "description", headerName: "Description", flex: 1 },
  { field: "dateCreated", headerName: "Date Created", flex: 1 },
  { field: "dateScheduled", headerName: "Date Scheduled", flex: 1 },
  { field: "taskPriority", headerName: "Task Priority", flex: 1 },
  {
    field: "taskStatus",
    editable: true,
    type: "singleSelect",
    valueOptions: ["New", "InProgress", "OnHold", "Done"],
  },
  {
    field: 'subTasks',
    headerName: 'Subtasks',
    width: 200,
    renderCell: (params) => {
      const subTasks = params.value || [];
      return (
        <ul>
          {subTasks.map((subTask) => (
            <li key={subTask.id}>{subTask.name}</li>
          ))}
        </ul>
      );
    },
  },
];

export default function MyTasks() {
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

  return (
    <>
      <div style={{ height: 400, width: "100%" }}>
        <DataGrid rows={tasks} columns={columns} />
      </div>
    </>
  );
}
