import { Button, Container, TextField, ThemeProvider, createTheme } from "@mui/material";
import styles from "./Task.module.css";
import { useState } from "react";

const theme = createTheme({
    components: {
      MuiButton: {
        styleOverrides: {
          root: {
            marginTop: 2,
          },
        },
      },
    },
  });

export default function Create(){
    const [taskData, setTaskData] = useState({
        id: 0,
        taskPriorityId: '',
        taskStatusId: '',
        name: '',
        dateCreated: '',
        dateScheduled: '',
        parentId: '',
        description: '',
      });
    
      const handleChange = (event) => {
        const { name, value } = event.target;
        setTaskData((prevTaskData) => ({
          ...prevTaskData,
          [name]: value,
        }));
      };
    
      const handleSubmit = (event) => {
        event.preventDefault();
        // Perform POST request with taskData
        console.log(taskData);
        // Reset form fields
        setTaskData({
          id: '',
          taskPriorityId: '',
          taskStatusId: '',
          name: '',
          dateCreated: '',
          dateScheduled: '',
          parentId: '',
          description: '',
        });
      };
    
      return (
        <ThemeProvider theme={theme}>
        <Container>
          <form className={styles.form} onSubmit={handleSubmit}>
            <TextField
              label="Task Priority ID"
              name="taskPriorityId"
              value={taskData.taskPriorityId}
              onChange={handleChange}
              required
            />
            <TextField
              label="Task Status ID"
              name="taskStatusId"
              value={taskData.taskStatusId}
              onChange={handleChange}
              required
            />
            <TextField
              label="Name"
              name="name"
              value={taskData.name}
              onChange={handleChange}
              required
            />
            <TextField
              label="Date Created"
              name="dateCreated"
              type="date"
              value={taskData.dateCreated}
              onChange={handleChange}
              required
              InputLabelProps={{
                shrink: true,
              }}
            />
            <TextField
              label="Date Scheduled"
              name="dateScheduled"
              type="date"
              value={taskData.dateScheduled}
              onChange={handleChange}
              required
              InputLabelProps={{
                shrink: true,
              }}
            />
            <TextField
              label="Parent ID"
              name="parentId"
              value={taskData.parentId}
              onChange={handleChange}
              required
            />
            <TextField
              label="Description"
              name="description"
              multiline
              rows={4}
              value={taskData.description}
              onChange={handleChange}
              required
            />
            <Button
              variant="contained"
              color="primary"
              type="submit"
            >
              Create Task
            </Button>
          </form>
        </Container>
        </ThemeProvider>
    )
}