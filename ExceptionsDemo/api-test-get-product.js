import { check } from "k6";
import http from "k6/http";

export const options = {
    stages: [
        { duration: "10s", target: 20 },
        { duration: "50s", target: 20 }
    ]
};

export default function () {
    const response = http.get("https://localhost:58556/api/products/1", {
        headers: { 'Content-Type': "application/json" }
    });

    check(response, {
        'response code was 404': (res) => res.status == 404
    });
}