import axios from 'axios';

export async function GetParts() {
    try {
        const response = await axios.get(`${process.env.REACT_APP_BACKEND}/parts`);

        return response.data;
    } catch (error) {
        console.error('Error fetching data from API:', error);
        return null;
    }
}

export async function PlaceOrder(order) {
    try {
        var body = JSON.stringify(order);
        console.log(body);
        const response = await axios.post(
            `${process.env.REACT_APP_BACKEND}/orders`,
            body,
            {
                headers: {
                    'Content-Type': 'application/json',
                },
            }
        );

        console.log(response);
        return response.data;
    } catch (error) {
        console.error('Error submitting data to API:', error);
        return null;
    }
}