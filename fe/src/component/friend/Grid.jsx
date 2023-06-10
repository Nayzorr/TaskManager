import { useEffect, useState } from "react"
import API from "../../api/axios";
import { GET_USER_FRIENDS } from "../../api/url";
import { Box } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";

const columns = [
  { field: "userName", headerName: "Username", width: 150 },
  { field: "email", headerName: "Email", width: 250 },
  { field: "firstName", headerName: "First Name", width: 130 },
  { field: "lastName", headerName: "Last Name", width: 130 },
  { field: "phoneNumber", headerName: "Phone Number", width: 150 },
];

export default function Grid(){
    const [friends, setFriends] = useState();

    useEffect(() => {
        API.get(GET_USER_FRIENDS)
        .then((response) => {
            console.log(response.data.data);
            setFriends(response.data.data)
          })
          .catch((erorr) => {
            if (erorr.response.status === 400) {
            }
          });
    }, []);

    return(
        <div>
          {friends ?
            <Box sx={{ height: 400, width: '100%' }}>
      <DataGrid
        rows={friends}
        columns={columns}
        initialState={{
          pagination: {
            paginationModel: {
              pageSize: 5,
            },
          },
        }}
        pageSizeOptions={[5]}
        disableRowSelectionOnClick
      />
    </Box>:"Loading..."}
        </div>
    )
}