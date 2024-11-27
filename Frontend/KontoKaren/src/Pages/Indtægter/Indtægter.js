import React, { useState } from "react"; // Sørg for at importere useState
import { Container, Typography } from "@mui/material";
import PieChart from "./PieChart";
import IndtægterTabel from "./IndtægterTabel"; // Importer tabellen

// Eksempeldata til tabellen
const initialRows = [
  { id: 1, name: "SU", price: 305, category: "1" },
  { id: 2, name: "Løn", price: 452, category: "2" },
  { id: 3, name: "Sort arbejde", price: 452, category: "hej" },
];

function Indtægter() {
  // Brug useState for at definere rows i komponentens tilstand
  const [rows, setRows] = useState(initialRows); // initialRows er dataene, der bruges som startværdi

  return (
    <Container>
      <Typography variant="h1" component="h2">
        Welcome to Indtægter
      </Typography>
      
      {/* Send rows data som prop til PieChart */}
      <PieChart chartData={rows} />

      {/* Send rows data og setRows funktion som props til IndtægterTabel */}
      <IndtægterTabel data={rows} setData={setRows} />
    </Container>
  );
}

export default Indtægter;
