import { Box } from '@mui/material';
import React from 'react';
import NavMenu from './NavMenu';

export default function Layout(props) {

  return (
    <Box>
      <NavMenu />
      <Box>
        {props.children}
      </Box>
    </Box>
  );
}
