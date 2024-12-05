import React from "react";
import {
  TableHead,
  TableRow,
  TableCell,
} from "@mui/material";

export default function TableHeader({
  numSelected,
}) {
  return (
    <TableHead>
      <TableRow>
        {/* Kolonnehoveder */}
        <TableCell style={{ width: '20%' }}>Indtægtsnavn</TableCell>
        <TableCell style={{ width: '20%' }}>Beløb</TableCell>
        <TableCell style={{ width: '20%' }}>Dato</TableCell>
        <TableCell style={{ width: '20%' }}>Handlinger</TableCell>
      </TableRow>
    </TableHead>
  );
}
