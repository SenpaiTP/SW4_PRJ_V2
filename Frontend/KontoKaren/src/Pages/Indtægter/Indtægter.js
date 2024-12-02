import React, { useState } from "react"; // Sørg for at importere useState
import { Container, Typography } from "@mui/material";
// import PieChart from "../../Components/IndtægterComponents/PieChart/PieChart";
import IndtægterTabel from "../../Components/IndtægterComponents/IndtægterComponents"; // Importer tabellen
import { initialRows } from "../../Components/IndtægterComponents/Table/TableData";

function Indtægter() {
  const [rows, setRows] = useState(initialRows);

  // const chartData = rows.map((row) => ({
  //   name: row.name,
  //   price: row.price,
  // }));

  return (
    <Container>
      <Typography variant="h1" component="h2" padding ="10" align = "center">
        Welcome to Indtægter
      </Typography>

      <IndtægterTabel data={rows} setData={setRows} />

      {/* <PieChart chartData={chartData} /> */}

    </Container>

  );
}

export default Indtægter;
