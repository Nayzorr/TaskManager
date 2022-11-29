import axios from "axios";

const instase = axios.create({
  baseURL: "http://localhost:7038/taskmanagerapi/api/",
});

export default instase;
