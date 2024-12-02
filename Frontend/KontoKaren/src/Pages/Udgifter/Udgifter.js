import React, { useState } from "react"; 
import { Container, Typography } from "@mui/material";
import UdgifterTable from "../../Components/UdgifterComponents/UdgifterComponents";
import { initialExpenseRows } from "../../Components/UdgifterComponents/Table/UdgifterTableData";

function Udgifter() {
  const [rows, setRows] = useState(initialExpenseRows);

  return (
    <Container>
      <Typography variant="h1" component="h2" padding ="10" align = "left">
        Udgifter
      </Typography>

      <UdgifterTable data={rows} setData={setRows} />

    </Container>

  );
}

export default Udgifter;
