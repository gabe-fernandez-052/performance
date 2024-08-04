import { sleep, check } from "k6";
import http from "k6/http";

export const options = {
    stages: [
        { duration: "10s", target: 20 }, //ramp-up
        { duration: "10s", target: 20 }, //stabilize
        { duration: "10s", target: 0 }   //ramp-down
    ],
    thresholds: {
        http_req_duration: ["p(99)<2s"] //99% of requests must complete within 2 seconds
    }
};

export default function () {
    const request = {
        id: "1",
        name: "",
        price: 0
    };

    const response = http.post("https://localhost:65252/api/products/", JSON.stringify(request), {
        headers: { 'Content-Type': "application/json" }
    });

    check(response, {
        'response code was 400': (res) => res.status == 400
    });

    sleep(1);
}