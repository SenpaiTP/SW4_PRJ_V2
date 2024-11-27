import React, { useState } from "react"; // Sørg for at importere useState
import { Container, Typography } from "@mui/material";
import PieChart from "./PieChart";
import IndtægterTabel from "./IndtægterTabel"; // Importer tabellen
import { DateRange } from "@mui/icons-material";

// Eksempeldata til tabellen
const initialRows = [
  { id: 1, name: "SU", price: 305, date: "2022-12-01" },
  { id: 2, name: "Løn", price: 120, date: "2022-12-01" },
  { id: 3, name: "Sort arbejde", price: 452, date: "2022-12-01" },
];

function Indtægter() {
  // Brug useState for at definere rows i komponentens tilstand
  const [rows, setRows] = useState(initialRows);

  return (
    <Container>
      <Typography variant="h1" component="h2">
        Welcome to Indtægter
      </Typography>

      {/* Send rows data og setRows funktion som props til IndtægterTabel */}
      <IndtægterTabel data={rows} setData={setRows} />
    </Container>
  );
}

export default Indtægter;
