import { default as axios} from 'axios';

export const routeService = {
    async searchRoutes({from, to}){
        const response = await axios.get(`api/routes/from/${from}/to/${to}`);
        return response.data;
    }
};
