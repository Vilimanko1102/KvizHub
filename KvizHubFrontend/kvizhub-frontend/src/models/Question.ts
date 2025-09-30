import { AnswerDto } from "./Answer";

export interface QuestionUpdateDto {
  id: number;
  text: string;
  type: "SingleChoice" | "MultipleChoice" | "FillIn";
  points: number;
  quizId: number;
}

export interface QuestionDto {
  id: number;
  text: string;
  type: string;
  points: number;
  answers?: AnswerDto[];
}