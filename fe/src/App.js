import Login from "./pages/login/Login";
import Dashboard from "./pages/dashboard/Dashboard";
import User from "./component/user/User";
import Friend from "./component/friend/Friend";
import Task from "./component/task/Task";
import Registration from "./pages/regisreation/Registration";
import "./App.css";
import { CookiesProvider } from "react-cookie";
import { Route, Routes } from "react-router-dom";
import RequiredAuth from "./component/auth/RequiredAuth";
import Grid from "./component/friend/Grid";
import CalendarComponent from "./component/calendar/CalendarComponent";
import Team from "./component/team/Team";

function App() {
  return (
    <div className="App">
      <CookiesProvider>
        <Routes>
          <Route path="login" element={<Login />} />
          <Route path="registration" element={<Registration />} />
          <Route element={<RequiredAuth />}>
            <Route path="dashboard" element={<Dashboard />}>
              <Route path="user" element={<User />} />
              <Route path="friends" element={<Friend />} />
              <Route path="tasks" element={<Task />} />
              <Route path="calendarComponent" element={<CalendarComponent />} />
              <Route path="team" element={<Team />} />
              <Route element={<Grid />} />
            </Route>
          </Route>
        </Routes>
      </CookiesProvider>
    </div>
  );
}

export default App;
