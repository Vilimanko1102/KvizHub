export interface QuizAttemptDto {
  id: number;
  userId: number;
  score: number;
  timeSpent: number;
  percentage: number;
  finishedAt: string;
  quizTitle: string;
}