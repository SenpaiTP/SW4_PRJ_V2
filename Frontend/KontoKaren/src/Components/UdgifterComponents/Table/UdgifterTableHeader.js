import React from "react";
import { TableHead, TableRow, TableCell } from "@mui/material";

export default function TableHeader({ numSelected }) {
  return (
    <TableHead>
      <TableRow>
        <TableCell style={{ width: '20%' }}>Udgiftsnavn</TableCell>
        <TableCell style={{ width: '20%' }} align="left">Kategori</TableCell>
        <TableCell style={{ width: '20%' }} align="left">Beløb</TableCell>
        <TableCell style={{ width: '20%' }} align="left">Dato</TableCell>
        <TableCell style={{ width: '20%' }} align="left">Handlinger</TableCell>
      </TableRow>
    </TableHead>
  );
}