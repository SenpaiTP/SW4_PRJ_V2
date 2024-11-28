import React, { useState } from "react"; // Sørg for at importere useState
import { Container, Typography } from "@mui/material";
import PieChart from "../../Components/IndtægterComponents/PieChart/PieChart";
import IndtægterTabel from "../../Components/IndtægterComponents/IndtægterTabel"; // Importer tabellen

// Eksempeldata til tabellen
const initialRows = [
  { id: 1, name: "SU", price: 305, date: "2022-12-01" },
  { id: 2, name: "Løn", price: 120, date: "2022-12-01" },
  { id: 3, name: "Sort arbejde", price: 452, date: "2022-12-01" },
];

function Indtægter() {
  // Brug useState for at definere rows i komponentens tilstand
  const [rows, setRows] = useState(initialRows);

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  return (
    <Container>
      <Typography variant="h1" component="h2">
        Welcome to Indtægter
      </Typography>

      <IndtægterTabel data={rows} setData={setRows} />

      //<PieChart chartData={chartData} />

    </Container>

  );
}

export default Indtægter;
