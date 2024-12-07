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
        {/* Checkbox for at vælge alle */}
        <TableCell padding="checkbox">
         
        </TableCell>

        {/* Kolonnehoveder */}
        <TableCell>Indtægtsnavn</TableCell>
        <TableCell align="left">Beløb</TableCell>
        <TableCell align="left">Dato</TableCell>
        <TableCell align="left">Handlinger</TableCell>
      </TableRow>
    </TableHead>
  );
}
