import { Divider, Typography } from "@mui/material";
import { Box } from "@mui/system";

export default function PersonalInfo() {
    const displayName = PersonalInfo.name;
    
    return (
        <Box mt={5}>
        <Typography variant="h1" component="h2">{displayName}</Typography>
        <Typography variant="h2" component="h3">Cloud Log</Typography>
        <Divider />
        <Typography>
            This is a cloud logbook application. It is a work in progress.
        </Typography>
        </Box>
    );
}