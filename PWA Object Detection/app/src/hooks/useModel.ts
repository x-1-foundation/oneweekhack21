import React, { useRef, useState } from "react";
import { ObjectDetectionModel } from "./../customVision";

const lastMatch: {
  [key: number]: { lastTop: number; lastLeft: number; timestamp: number };
} = {};

export const useModel = (imgId: string) => {
  const [loading, setLoading] = useState(true);
  const model = React.useRef(new ObjectDetectionModel());
  const img = useRef<HTMLElement | null>();

  React.useEffect(() => {
    model.current.loadModelAsync("model/model.json").then(() => {
      setLoading(false);
    });
  });

  const startDetection = async () => {
    try {
      if (!img.current) {
        img.current = document.getElementById(imgId);
      } else {
        const result = await model.current.executeAsync(img.current);
        const [detected_boxes, detected_scores, detected_classes] = result || [
          [],
          [],
          [],
        ];

        const highlighter = document.getElementById("highlighter");
        highlighter!.innerHTML = ""; //"<h1>" + Date.now() + "</h1>";
        detected_scores.map((score: number, i: number) => {
          //console.log(score);

          if (score > 0.43) {
            console.log(score);
            const p = document.createElement("p");
            const imageView = img.current!;
            const box = detected_boxes[i];

            const boxLeft = box[0] * imageView.clientWidth;
            const boxTop = box[1] * imageView.clientHeight;
            const boxWidth = imageView.clientWidth * box[2] - boxLeft;
            const boxheight = imageView.clientHeight * box[3] - boxTop;

            p.setAttribute("class", "highlighter");

            (p.style as any) =
              "background-color:rgba(0, 255, 0, 0.25);  border: 1px dashed #fff; position: absolute;left:" +
              boxLeft +
              "px;top:" +
              boxTop +
              "px;width: " +
              boxWidth +
              "px;height:" +
              boxheight +
              "px;";

            highlighter!.appendChild(p);

            const { lastTop, lastLeft, timestamp } = lastMatch[
              detected_classes[i]
            ] ?? {
              lastTop: 0,
              lastLeft: 0,
              timestamp: 0,
            };
            if (Date.now() > timestamp + 5000 || boxTop > lastTop + 15) {
              console.log("new match");
            }
            lastMatch[detected_classes[i]] = {
              lastTop: boxTop,
              lastLeft: boxLeft,
              timestamp: Date.now(),
            };
          }
        });
      }
      window.requestAnimationFrame(startDetection);
    } catch (ex) {
      console.log("Cam is not ready yet");
      window.requestAnimationFrame(startDetection);
    }
  };

  return {
    loading,
    startDetection,
  };
};
