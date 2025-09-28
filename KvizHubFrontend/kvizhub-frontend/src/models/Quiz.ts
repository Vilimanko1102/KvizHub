export interface Quiz {
  id: number;
  title: string;
  description: string;
  category: string;
  difficulty: string;
  timeLimit: number;      // in minutes
  questionCount: number;
  isPlayable: boolean;
}

// Za kreiranje kviza (bez id i questionCount jer se generišu na backendu)
export interface QuizCreateDto {
  title: string;
  description: string;
  category: string;
  difficulty: string;
  timeLimit: number;
  createdBy: number
}

// Za update kviza (sva polja opciona)
export interface QuizUpdateDto {
  title?: string;
  description?: string;
  category?: string;
  difficulty?: string;
  timeLimit?: number;
}