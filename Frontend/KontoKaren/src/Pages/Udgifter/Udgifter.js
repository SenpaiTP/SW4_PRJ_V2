import React from "react";
import { Container, Typography } from "@mui/material";
import UdgifterTableComponent from "../../Components/UdgifterComponents/UdgifterTableComponent";
import { initialExpenseRows } from "../../Components/UdgifterComponents/Table/VariableUdgifterTableData";
import { initialFixedExpenseRows } from "../../Components/UdgifterComponents/Table/FasteUdgifterTableData";
import { getVudgifter, createVudgifter, updateVudgifter, deleteVudgifter } from "../../Services/VariableUdgifterService";
import { getFudgifter, createFudgifter, updateFudgifter, deleteFudgifter } from "../../Services/FasteUdgifterService";
import AmountLimitComponent from "../../Components/UdgifterComponents/AmountLimitComponent";



function Udgifter() {
  return (
    <Container>
      <Typography variant="h1" component="h2" padding="10" align="left">
        Udgifter
      </Typography>

      <AmountLimitComponent/>

      <UdgifterTableComponent
        title="Variable Udgifter"
        initialRows={initialExpenseRows}
        storageKey="variableUdgifterRows" // Unique storageKey for variable
        fetchFunction={getVudgifter}
        createFunction={createVudgifter}
        updateFunction={updateVudgifter}
        deleteFunction={deleteVudgifter}
      />
      <UdgifterTableComponent
        title="Faste Udgifter"
        initialRows={initialFixedExpenseRows}
        storageKey="fasteUdgifterRows" // Unique storageKey for fixed
        fetchFunction={getFudgifter}
        createFunction={createFudgifter}
        updateFunction={updateFudgifter}
        deleteFunction={deleteFudgifter}
      />
    </Container>
  );
}

export default Udgifter;