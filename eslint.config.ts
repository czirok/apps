import antfu from '@antfu/eslint-config'

export default antfu(
	{
		typescript: true,
		yaml: true,
		jsonc: true,
		markdown: false,
		formatters: {
			css: false,
			markdown: false,
			xml: true,
		},
		stylistic: {
			indent: 'tab',
			quotes: 'single',
		},
		ignores: [
			'**/dist',
			'**/node_modules',
			'**/*.js',
			'**/*.d.ts',
			'.vscode/.extensions',
			'.vscode/.user.data',
			'.vscode/.linux/.dotnet',
			'.pnpm-lock.yaml',
		],
		rules: {
			'unused-imports/no-unused-imports': 'warn',
		},
	},
	{
		files: ['**/*.props', '**/*.csproj', '**/*.pubxml'],
		rules: {
			'style/indent': [2, 2],
			'style/jsx-indent-props': [2, 2],
			'style/eol-last': ['error', 'never'],
		},
	},
)
