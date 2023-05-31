import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Login from "./pages/login/Login";
import Dashboard from "./pages/dashboard/Dashboard";
import User from "./component/user/User";
import Friend from "./component/friend/Friend";
import Task from "./component/task/Task"

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <BrowserRouter>
    <Routes>
      <Route index element = {<App />} />
      <Route path="login" element= {<Login/>} />
      <Route path="dashboard" element={<Dashboard />}>
              <Route path="user" element= {<User/>} />
              <Route path="friends" element= {<Friend/>} />
              <Route path="tasks" element = {<Task />} />
      </Route>
    </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
