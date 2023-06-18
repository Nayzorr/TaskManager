import { DataGrid } from "@mui/x-data-grid";
import { Button } from "@mui/material";
import API from "../../api/axios";
import { useState } from "react";
import { useEffect } from "react";

export default function WantToConnect() {
  const [data, setData] = useState([]);

  const updateInfo = () => {
    API.get("Team/GetMyTeamInvitations").then((response) => {
        setData(response.data.data);
        console.log(response.data.data);
      });
  }

  useEffect(() => {
    updateInfo();
  }, []);

  const columns = [
    { field: "teamName", headerName: "Team Name", width: 200 },
    { field: "creatorId", headerName: "Creator ID", width: 150 },
    { field: "dateCreated", headerName: "Date Created", width: 200 },
    {
      field: "actions",
      headerName: "Actions",
      width: 200,
      renderCell: (params) => {
        const handleAccept = () => {
          API.put(`Team/AcceptTeamInvitation?teamName=${params.row.teamName}`);
          updateInfo();
        };
  
        const handleCancel = () => {
          API.put(`Team/RejectTeamInvitation?teamName=${params.row.teamName}`);
          updateInfo();
        };
  
        return (
          <div>
            <Button variant="contained" color="primary" onClick={handleAccept}>
              Accept
            </Button>
            <Button variant="contained" color="secondary" onClick={handleCancel}>
              Cancel
            </Button>
          </div>
        );
      },
    },
  ];

  return (
    <div style={{ height: 400, width: "100%" }}>
      <DataGrid rows={data} columns={columns} />
    </div>
  );
}
