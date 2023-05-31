import Login from "./pages/login/Login";
import "./App.css";
import Registration from "./pages/regisreation/Registration";
import Dashboard from "./pages/dashboard/Dashboard";
import { createBrowserRouter } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Login />
    </div>
  );
}

export default App;
