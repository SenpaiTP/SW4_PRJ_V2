import React, { useState } from "react"; 
import { Container, Typography } from "@mui/material";
import IndtægterTabel from "../../Components/IndtægterComponents/IndtægterComponents"; 
import { initialRows } from "../../Components/IndtægterComponents/Table/TableData";

function Indtægter() {
  const [rows, setRows] = useState(initialRows);

  return (
    <Container>
      <Typography variant="h1" component="h2" padding ="10" align = "left">
        Indtægter
      </Typography>

      <IndtægterTabel data={rows} setData={setRows} />

    </Container>

  );
}

export default Indtægter;
